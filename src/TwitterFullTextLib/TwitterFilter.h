// TwitterFilter.h : Declaration of the CTwitterFilter

#pragma once
#include "resource.h"       // main symbols



#include "TwitterFullTextLib_i.h"



#if defined(_WIN32_WCE) && !defined(_CE_DCOM) && !defined(_CE_ALLOW_SINGLE_THREADED_OBJECTS_IN_MTA)
#error "Single-threaded COM objects are not properly supported on Windows CE platform, such as the Windows Mobile platforms that do not include full DCOM support. Define _CE_ALLOW_SINGLE_THREADED_OBJECTS_IN_MTA to force ATL to support creating single-thread COM object's and allow use of it's single-threaded COM object implementations. The threading model in your rgs file was set to 'Free' as that is the only threading model supported in non DCOM Windows CE platforms."
#endif

using namespace ATL;


// CTwitterFilter

class ATL_NO_VTABLE CTwitterFilter :
	public CComObjectRootEx<CComMultiThreadModel>,
	public CComCoClass<CTwitterFilter, &CLSID_TwitterFilter>,
	public IDispatchImpl<ITwitterFilter, &IID_ITwitterFilter, &LIBID_TwitterFullTextLib, /*wMajor =*/ 1, /*wMinor =*/ 0>,
	public IFilter,
	public IPersistStream,
	public IPersistFile
{
public:
	CTwitterFilter();

DECLARE_REGISTRY_RESOURCEID(IDR_TWITTERFILTER)


BEGIN_COM_MAP(CTwitterFilter)
	COM_INTERFACE_ENTRY(ITwitterFilter)
	COM_INTERFACE_ENTRY(IDispatch)
	COM_INTERFACE_ENTRY(IFilter) 
	COM_INTERFACE_ENTRY(IPersistStream) 
	COM_INTERFACE_ENTRY(IPersistFile) 
END_COM_MAP()



	DECLARE_PROTECT_FINAL_CONSTRUCT()

	HRESULT FinalConstruct()
	{
		return S_OK;
	}

	void FinalRelease()
	{
	}

public:

		// IFilter methods

	virtual  SCODE STDMETHODCALLTYPE  Init( ULONG grfFlags,
                                            ULONG cAttributes,
                                            FULLPROPSPEC const * aAttributes,
                                            ULONG * pFlags );
 
    virtual  SCODE STDMETHODCALLTYPE  GetChunk( STAT_CHUNK * pStat);
 
    virtual  SCODE STDMETHODCALLTYPE  GetText( ULONG * pcwcBuffer,
                                               WCHAR * awcBuffer );
 
    virtual  SCODE STDMETHODCALLTYPE  GetValue( PROPVARIANT * * ppPropValue );
 
    virtual  SCODE STDMETHODCALLTYPE  BindRegion( FILTERREGION origPos,
                                                  REFIID riid,
                                                  void ** ppunk);
 
	// IPersistStream methods

	virtual  SCODE STDMETHODCALLTYPE  IsDirty();

	virtual HRESULT STDMETHODCALLTYPE GetSizeMax( 
        __RPC__out ULARGE_INTEGER *pcbSize);

	virtual HRESULT STDMETHODCALLTYPE Load( 
        __RPC__in_opt IStream *pStm);
    
    virtual HRESULT STDMETHODCALLTYPE Save( 
        __RPC__in_opt IStream *pStm,
        BOOL fClearDirty);

	// IPersistFile methods

    virtual  SCODE STDMETHODCALLTYPE  GetClassID( CLSID * pClassID );

	virtual  SCODE STDMETHODCALLTYPE  Load( LPCWSTR pszFileName,
                                            DWORD dwMode);
 
    virtual  SCODE STDMETHODCALLTYPE  Save( LPCWSTR pszFileName,
                                            BOOL fRemember );
 
    virtual  SCODE STDMETHODCALLTYPE  SaveCompleted( LPCWSTR pszFileName );
 
    virtual  SCODE STDMETHODCALLTYPE  GetCurFile( LPWSTR  * ppszFileName );


private:
    IStream*                    m_pStream;         // Stream of this document
    DWORD                       m_dwChunkId;        // Current chunk id

	HRESULT FilterAndCopy(LPCWSTR source, LPWSTR dest, ULONG* count);


};

OBJECT_ENTRY_AUTO(__uuidof(TwitterFilter), CTwitterFilter)
