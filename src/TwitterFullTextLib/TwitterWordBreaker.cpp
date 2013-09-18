// TwitterWordBreaker.cpp : Implementation of CTwitterWordBreaker

#include "stdafx.h"
#include "TwitterWordBreaker.h"


// CTwitterWordBreaker

HRESULT CTwitterWordBreaker::Init(BOOL fQuery, ULONG ulMaxTokenSize, BOOL *pfLicense)
{
	if (pfLicense == 0)
	{
		return E_INVALIDARG;
	}

    m_ulMaxTokenSize = ulMaxTokenSize;
    *pfLicense = FALSE;

	return S_OK;
}

HRESULT CTwitterWordBreaker::BreakText(TEXT_SOURCE *pTextSource, IWordSink *pWordSink, IPhraseSink *pPhraseSink)
{
	// Validate arguments
    if (pTextSource == 0)
	{
        return E_INVALIDARG;
	}

    if (pWordSink == 0 || (pTextSource->iCur == pTextSource->iEnd))
	{
        return S_OK;
	}

    if (pTextSource->iCur > pTextSource->iEnd)
	{
        return E_INVALIDARG;
	}

	ULONG ulProcessed;   // # chars actually processed by Tokenize()
    HRESULT hr = S_OK;

    // Pull text from the text source and tokenize it
	LPCWSTR source = (pTextSource->awcBuffer + pTextSource->iCur);
	ULONG sc = pTextSource->iEnd - pTextSource->iCur;

	FilterAndCopy(source, sc, pWordSink);
	
	return S_OK;
}


HRESULT CTwitterWordBreaker::FilterAndCopy(LPCWSTR source, ULONG count, IWordSink* pWordSink)
{
	const LPWSTR whitespace = L" \t\n\r";
	const LPWSTR punctuation = L",.!?;:\"-/+(){}[]\\`´_^~";

	ULONG ws = 0;		// word start
	ULONG wc = 0;		// word letter count
	WCHAR ll = '\0';	// last letter processed
	ULONG lc = 0;		// same letter count
	BOOL isnoisy = false;
	BOOL isurl = false;
	BYTE* bsource = (BYTE*)source;
	int i = 0;

	WCHAR c;
	BYTE bu;
	BYTE bl;
	BOOL isWhitespace;
	BOOL isPunctuation;

	if (count > 2)
	{
		// Check unicode signature, skip if any
		if (bsource[0] == 0xFF || bsource[0] == 0xFE)
		{
			i = 1;
		}
	}
	else
	{
		count = 0;
		return S_OK;
	}

	// Read input letter by letter and put together into the word buffer
	for (; i <= count; i ++)
	{
		if (i == count)
		{
			c = L'\0';
		}
		else
		{
			c = source[i];
			bu = bsource[2 * i];
			bl = bsource[2 * i + 1];		

			isWhitespace = StrChr(whitespace, c) != 0;
			isPunctuation = StrChr(punctuation, c) != 0;
		}

		if (isWhitespace || isPunctuation || c == L'\0')
		{
			if (wc > 3 && !isnoisy && !isurl)
			{
				pWordSink->PutWord(wc, source + ws, wc, ws);
				//pWordSink->PutBreak(WORDREP_BREAK_EOW);
			}

			wc = 0;
			isnoisy = false;
			if (isWhitespace)
			{
				isurl = false;
			}

			if (c == L'\0')
			{
				break;
			}
		}
		else if (i < count - 4 &&
			(source[i] == 'h' || source[i] == 'H') &&
			(source[i + 1] == 't' || source[i + 1] == 'T') &&
			(source[i + 2] == 't' || source[i + 2] == 'T') &&
			(source[i + 3] == 'p' || source[i + 3] == 'P'))
		{
			isurl = true;
		}
		else
		{
			if (c == ll || c == ll + 0x20 || c == ll - 0x20)	// do not distinguish upper and lower case, might cause some false negatives, but only a few
			{
				lc++;
			}
			else
			{
				ll = c;
				lc = 1;
			}

			isnoisy |= lc >= 3;							// letter repeated more than trice

			isnoisy |= (bl > 0x05);						// Not latin greek or cyrillic
			isnoisy |= (bl == 0x00 && bu < 0x41);					// Symbols
			isnoisy |= (bl == 0x00 && 0x7B <= bu && bu < 0xC0);		// More symbols
			isnoisy |= (bl == 0x03 && bu < 0x80);		// More symbols

			// append to the word
			if (wc == 0)
			{
				ws = i;
			}
			wc++;
		}
	}

	return S_OK;
}

HRESULT CTwitterWordBreaker::ComposePhrase(const WCHAR *pwcNoun, ULONG cwcNoun, const WCHAR *pwcModifier, ULONG cwcModifier, ULONG ulAttachmentType, WCHAR *pwcPhrase, ULONG *pcwcPhrase)
{
	return E_NOTIMPL;
}

HRESULT CTwitterWordBreaker::GetLicenseToUse(const WCHAR **ppwcsLicense)
{
	if (ppwcsLicense == 0)
	{
		return E_INVALIDARG;
	}

	*ppwcsLicense = L"Twitter Toolkit";
	return S_OK;
}

