// TwitterFilter.cpp : Implementation of CTwitterFilter

#include "stdafx.h"
#include "TwitterFilter.h"
#include <filter.h>
#include <filterr.h>
#include <propkey.h>
#include <propsys.h>
#include <propvarutil.h>

// CTwitterFilter

CTwitterFilter::CTwitterFilter()
{
	m_pStream = 0;
}

SCODE STDMETHODCALLTYPE CTwitterFilter::Init(
	ULONG grfFlags,
	ULONG cAttributes,
	FULLPROPSPEC const * aAttributes,
	ULONG * pFlags )
{
    //This pointer is not set to any value. If you do not set it to 0
    //the IFilter will not work.
    *pFlags = 0;
 
    // Common initialization.
    m_dwChunkId = 0;
   
    return S_OK;
}

SCODE STDMETHODCALLTYPE CTwitterFilter::GetChunk( STAT_CHUNK * pStat)
{ 
	ZeroMemory(pStat, sizeof(STAT_CHUNK));

	if (m_dwChunkId == 0)
	{
		pStat->idChunk = m_dwChunkId++;
		pStat->breakType = CHUNK_NO_BREAK;	// end of sentence
		pStat->flags = CHUNK_TEXT;
		pStat->locale = LOCALE_NEUTRAL;

		pStat->attribute.guidPropSet = PKEY_Search_Contents.fmtid;
		pStat->attribute.psProperty.ulKind = PRSPEC_PROPID;
		pStat->attribute.psProperty.propid = 1;
	
		pStat->idChunkSource = 0;
		pStat->cwcStartSource = 0;
		pStat->cwcLenSource = 0;

		return S_OK;
	}
	else
	{
		return FILTER_E_END_OF_CHUNKS;
	}
}

SCODE STDMETHODCALLTYPE CTwitterFilter::GetText( ULONG * pcwcBuffer,
    WCHAR * awcBuffer )
{ 
	BYTE buffer[0x1000];
    BOOL eof = false;
    ULONG bytesRead;
    HRESULT hr;

	hr = m_pStream->Read(buffer, 0xFFE, &bytesRead);
	
	if (bytesRead > 0)
	{
		buffer[bytesRead] = 0;
		buffer[bytesRead + 1] = 0;

		LPWSTR text = (LPWSTR)buffer;
		ULONG length = (bytesRead + 2) / 2;		// assume unicode

		hr = FilterAndCopy(text, awcBuffer, pcwcBuffer);

		if (FAILED(hr))
		{
			return FILTER_E_UNKNOWNFORMAT;
		}
		else
		{
			return FILTER_S_LAST_TEXT;
		}
	}
	else
	{
		return FILTER_E_NO_MORE_TEXT;
	}
}

HRESULT CTwitterFilter::FilterAndCopy(LPCWSTR source, LPWSTR dest, ULONG* count)
{
	const LPWSTR whitespace = L" \t\n\r";
	const LPWSTR punctuation = L",.!?;:\"'-/+(){}[]";

	WCHAR word[0x1000];
	ULONG wc = 0;
	ULONG rc = 0;
	WCHAR ll = '\0';
	ULONG lc = 0;
	BOOL isnoisy = false;
	BOOL isurl = false;
	BYTE* bsource = (BYTE*)source;
	int i = 0;

	if (*count > 2)
	{
		// Check unicode signature, skip if any
		if (bsource[0] == 0xFF || bsource[0] == 0xFE)
		{
			i = 1;
		}
	}
	else
	{
		*count = 0;
		return FILTER_E_NO_MORE_TEXT;
	}

	// Read input letter by letter and put together into the word buffer
	for (; i < *count; i ++)
	{
		WCHAR c = source[i];
		BYTE bu = bsource[2 * i];
		BYTE bl = bsource[2 * i + 1];		

		BOOL isWhitespace = StrChr(whitespace, c) != 0;
		BOOL isPunctuation = StrChr(punctuation, c) != 0;

		if (isWhitespace || isPunctuation || c == L'\0')
		{
			if (wc > 3 && !isnoisy && !isurl)
			{
				// End of word reached, copy to output
				for (int k = 0; k < wc; k++)
				{
					dest[rc++] = word[k];
				}

				dest[rc++] = ' ';
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
		else if (i < *count - 4 &&
			(source[i] == 'h' || source[i] == 'H') &&
			(source[i + 1] == 't' || source[i + 1] == 'T') &&
			(source[i + 2] == 't' || source[i + 2] == 'T') &&
			(source[i + 3] == 'p' || source[i + 3] == 'P'))
		{
			isurl = true;
		}
		else
		{
			if (c == ll)
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
			isnoisy |= (bl == 0x00 && 0x7B <= bu && bu <= 0xBF);		// More symbols

			// append to the word
			word[wc++] = source[i];
		}
	}

	dest[rc] = '\0';
	*count = rc;

	return S_OK;
}

SCODE STDMETHODCALLTYPE CTwitterFilter::GetValue( PROPVARIANT * * ppPropValue )
{ 
    return E_NOTIMPL;
}

SCODE STDMETHODCALLTYPE CTwitterFilter::BindRegion( FILTERREGION origPos,
  REFIID riid,
  void ** ppunk)
{ 
    return E_NOTIMPL;
}

// IPersistStream methods

SCODE STDMETHODCALLTYPE CTwitterFilter::IsDirty()
{
    return E_NOTIMPL;
}

HRESULT STDMETHODCALLTYPE CTwitterFilter::GetSizeMax(__RPC__out ULARGE_INTEGER *pcbSize)
{
    return E_NOTIMPL;
}

HRESULT STDMETHODCALLTYPE CTwitterFilter::Load( __RPC__in_opt IStream *pStm)
{ 
    if (m_pStream)
    {
        m_pStream->Release();
    }
    m_pStream = pStm;
    m_pStream->AddRef();
    return S_OK;
}

HRESULT STDMETHODCALLTYPE CTwitterFilter::Save(__RPC__in_opt IStream *pStm, BOOL fClearDirty)
{ 
    return E_NOTIMPL;
}

// IPersistFile methods

SCODE STDMETHODCALLTYPE CTwitterFilter::GetClassID( CLSID * pClassID )
{
    return E_NOTIMPL;
}

SCODE STDMETHODCALLTYPE CTwitterFilter::Load( LPCWSTR pszFileName,
  DWORD dwMode)
{    
    IStream *stream;
    USES_CONVERSION;
    HRESULT hResult = SHCreateStreamOnFile(pszFileName, STGM_READ, &stream);
    if (FAILED (hResult))
    {
        return hResult;
    }
    // Use the load method for the stream.
    return Load(stream); 
}
 
SCODE STDMETHODCALLTYPE CTwitterFilter::Save( LPCWSTR pszFileName,
  BOOL fRemember )
{
    return E_NOTIMPL;
}
 
SCODE STDMETHODCALLTYPE CTwitterFilter::SaveCompleted( LPCWSTR pszFileName )
{
    return E_NOTIMPL;
}
 
SCODE STDMETHODCALLTYPE CTwitterFilter::GetCurFile( LPWSTR  * ppszFileName )
{ 
    return E_NOTIMPL;
}