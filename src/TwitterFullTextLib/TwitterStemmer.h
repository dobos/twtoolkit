// TwitterStemmer.h : Declaration of the CTwitterStemmer

#pragma once
#include "resource.h"       // main symbols



#include "TwitterFullTextLib_i.h"



#if defined(_WIN32_WCE) && !defined(_CE_DCOM) && !defined(_CE_ALLOW_SINGLE_THREADED_OBJECTS_IN_MTA)
#error "Single-threaded COM objects are not properly supported on Windows CE platform, such as the Windows Mobile platforms that do not include full DCOM support. Define _CE_ALLOW_SINGLE_THREADED_OBJECTS_IN_MTA to force ATL to support creating single-thread COM object's and allow use of it's single-threaded COM object implementations. The threading model in your rgs file was set to 'Free' as that is the only threading model supported in non DCOM Windows CE platforms."
#endif

using namespace ATL;


// CTwitterStemmer

class ATL_NO_VTABLE CTwitterStemmer :
	public CComObjectRootEx<CComMultiThreadModel>,
	public CComCoClass<CTwitterStemmer, &CLSID_TwitterStemmer>,
	public IDispatchImpl<ITwitterStemmer, &IID_ITwitterStemmer, &LIBID_TwitterFullTextLib, /*wMajor =*/ 1, /*wMinor =*/ 0>,
	public IStemmer
{
public:
	CTwitterStemmer()
	{
	}

DECLARE_REGISTRY_RESOURCEID(IDR_TWITTERSTEMMER)


BEGIN_COM_MAP(CTwitterStemmer)
	COM_INTERFACE_ENTRY(ITwitterStemmer)
	COM_INTERFACE_ENTRY(IDispatch)
	COM_INTERFACE_ENTRY(IStemmer)
END_COM_MAP()



	DECLARE_PROTECT_FINAL_CONSTRUCT()

	HRESULT FinalConstruct()
	{
		return S_OK;
	}

	void FinalRelease()
	{
	}

private:

	ULONG m_ulMaxTokenSize;

public:

	virtual HRESULT STDMETHODCALLTYPE Init(ULONG ulMaxTokenSize, BOOL *pfLicense);
    virtual HRESULT STDMETHODCALLTYPE GenerateWordForms(const WCHAR *pwcInBuf, ULONG cwc, IWordFormSink *pStemSink);
    virtual HRESULT STDMETHODCALLTYPE GetLicenseToUse(const WCHAR **ppwcsLicense);




};

OBJECT_ENTRY_AUTO(__uuidof(TwitterStemmer), CTwitterStemmer)
