// TwitterStemmer.cpp : Implementation of CTwitterStemmer

#include "stdafx.h"
#include "TwitterStemmer.h"


// CTwitterStemmer

HRESULT CTwitterStemmer::Init(ULONG ulMaxTokenSize, BOOL *pfLicense)
{
	m_ulMaxTokenSize = ulMaxTokenSize;
	*pfLicense = false;

	return S_OK;
}

HRESULT CTwitterStemmer::GenerateWordForms(const WCHAR *pwcInBuf, ULONG cwc, IWordFormSink *pStemSink)
{
	if (pwcInBuf == 0|| pStemSink == 0)
        return E_INVALIDARG;

	// return original word
	return pStemSink->PutWord(pwcInBuf, cwc);
}

HRESULT CTwitterStemmer::GetLicenseToUse(const WCHAR **ppwcsLicense)
{
	if (ppwcsLicense == 0)
	{
		return E_INVALIDARG;
	}

	*ppwcsLicense = L"Twitter Toolkit";

	return S_OK;
}