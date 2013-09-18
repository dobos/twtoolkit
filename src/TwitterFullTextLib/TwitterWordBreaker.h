// TwitterWordBreaker.h : Declaration of the CTwitterWordBreaker

#pragma once
#include "resource.h"       // main symbols



#include "TwitterFullTextLib_i.h"



#if defined(_WIN32_WCE) && !defined(_CE_DCOM) && !defined(_CE_ALLOW_SINGLE_THREADED_OBJECTS_IN_MTA)
#error "Single-threaded COM objects are not properly supported on Windows CE platform, such as the Windows Mobile platforms that do not include full DCOM support. Define _CE_ALLOW_SINGLE_THREADED_OBJECTS_IN_MTA to force ATL to support creating single-thread COM object's and allow use of it's single-threaded COM object implementations. The threading model in your rgs file was set to 'Free' as that is the only threading model supported in non DCOM Windows CE platforms."
#endif

using namespace ATL;


// CTwitterWordBreaker

class ATL_NO_VTABLE CTwitterWordBreaker :
	public CComObjectRootEx<CComMultiThreadModel>,
	public CComCoClass<CTwitterWordBreaker, &CLSID_TwitterWordBreaker>,
	public IDispatchImpl<ITwitterWordBreaker, &IID_ITwitterWordBreaker, &LIBID_TwitterFullTextLib, /*wMajor =*/ 1, /*wMinor =*/ 0>,
	public IWordBreaker
{
public:
	CTwitterWordBreaker()
	{
	}

DECLARE_REGISTRY_RESOURCEID(IDR_TWITTERWORDBREAKER)


BEGIN_COM_MAP(CTwitterWordBreaker)
	COM_INTERFACE_ENTRY(ITwitterWordBreaker)
	COM_INTERFACE_ENTRY(IDispatch)
	COM_INTERFACE_ENTRY(IWordBreaker)
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

	HRESULT FilterAndCopy(LPCWSTR source, ULONG count, IWordSink* pWordSink);

public:

	virtual HRESULT STDMETHODCALLTYPE Init(BOOL fQuery, ULONG ulMaxTokenSize, BOOL *pfLicense);
    virtual HRESULT STDMETHODCALLTYPE BreakText(TEXT_SOURCE *pTextSource, IWordSink *pWordSink, IPhraseSink *pPhraseSink);
    virtual HRESULT STDMETHODCALLTYPE ComposePhrase(const WCHAR *pwcNoun, ULONG cwcNoun, const WCHAR *pwcModifier, ULONG cwcModifier, ULONG ulAttachmentType, WCHAR *pwcPhrase, ULONG *pcwcPhrase);
    virtual HRESULT STDMETHODCALLTYPE GetLicenseToUse(const WCHAR **ppwcsLicense);



};

OBJECT_ENTRY_AUTO(__uuidof(TwitterWordBreaker), CTwitterWordBreaker)
