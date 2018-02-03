

/* this ALWAYS GENERATED file contains the definitions for the interfaces */


 /* File created by MIDL compiler version 8.00.0603 */
/* at Fri Oct 30 15:28:55 2015
 */
/* Compiler settings for TwitterFullTextLib.idl:
    Oicf, W1, Zp8, env=Win64 (32b run), target_arch=AMD64 8.00.0603 
    protocol : dce , ms_ext, c_ext, robust
    error checks: allocation ref bounds_check enum stub_data 
    VC __declspec() decoration level: 
         __declspec(uuid()), __declspec(selectany), __declspec(novtable)
         DECLSPEC_UUID(), MIDL_INTERFACE()
*/
/* @@MIDL_FILE_HEADING(  ) */

#pragma warning( disable: 4049 )  /* more than 64k source lines */


/* verify that the <rpcndr.h> version is high enough to compile this file*/
#ifndef __REQUIRED_RPCNDR_H_VERSION__
#define __REQUIRED_RPCNDR_H_VERSION__ 475
#endif

#include "rpc.h"
#include "rpcndr.h"

#ifndef __RPCNDR_H_VERSION__
#error this stub requires an updated version of <rpcndr.h>
#endif // __RPCNDR_H_VERSION__

#ifndef COM_NO_WINDOWS_H
#include "windows.h"
#include "ole2.h"
#endif /*COM_NO_WINDOWS_H*/

#ifndef __TwitterFullTextLib_i_h__
#define __TwitterFullTextLib_i_h__

#if defined(_MSC_VER) && (_MSC_VER >= 1020)
#pragma once
#endif

/* Forward Declarations */ 

#ifndef __IWordSink_FWD_DEFINED__
#define __IWordSink_FWD_DEFINED__
typedef interface IWordSink IWordSink;

#endif 	/* __IWordSink_FWD_DEFINED__ */


#ifndef __IPhraseSink_FWD_DEFINED__
#define __IPhraseSink_FWD_DEFINED__
typedef interface IPhraseSink IPhraseSink;

#endif 	/* __IPhraseSink_FWD_DEFINED__ */


#ifndef __IWordBreaker_FWD_DEFINED__
#define __IWordBreaker_FWD_DEFINED__
typedef interface IWordBreaker IWordBreaker;

#endif 	/* __IWordBreaker_FWD_DEFINED__ */


#ifndef __IWordFormSink_FWD_DEFINED__
#define __IWordFormSink_FWD_DEFINED__
typedef interface IWordFormSink IWordFormSink;

#endif 	/* __IWordFormSink_FWD_DEFINED__ */


#ifndef __IStemmer_FWD_DEFINED__
#define __IStemmer_FWD_DEFINED__
typedef interface IStemmer IStemmer;

#endif 	/* __IStemmer_FWD_DEFINED__ */


#ifndef __ITwitterWordBreaker_FWD_DEFINED__
#define __ITwitterWordBreaker_FWD_DEFINED__
typedef interface ITwitterWordBreaker ITwitterWordBreaker;

#endif 	/* __ITwitterWordBreaker_FWD_DEFINED__ */


#ifndef __ITwitterStemmer_FWD_DEFINED__
#define __ITwitterStemmer_FWD_DEFINED__
typedef interface ITwitterStemmer ITwitterStemmer;

#endif 	/* __ITwitterStemmer_FWD_DEFINED__ */


#ifndef __ITwitterFilter_FWD_DEFINED__
#define __ITwitterFilter_FWD_DEFINED__
typedef interface ITwitterFilter ITwitterFilter;

#endif 	/* __ITwitterFilter_FWD_DEFINED__ */


#ifndef __TwitterWordBreaker_FWD_DEFINED__
#define __TwitterWordBreaker_FWD_DEFINED__

#ifdef __cplusplus
typedef class TwitterWordBreaker TwitterWordBreaker;
#else
typedef struct TwitterWordBreaker TwitterWordBreaker;
#endif /* __cplusplus */

#endif 	/* __TwitterWordBreaker_FWD_DEFINED__ */


#ifndef __TwitterStemmer_FWD_DEFINED__
#define __TwitterStemmer_FWD_DEFINED__

#ifdef __cplusplus
typedef class TwitterStemmer TwitterStemmer;
#else
typedef struct TwitterStemmer TwitterStemmer;
#endif /* __cplusplus */

#endif 	/* __TwitterStemmer_FWD_DEFINED__ */


#ifndef __TwitterFilter_FWD_DEFINED__
#define __TwitterFilter_FWD_DEFINED__

#ifdef __cplusplus
typedef class TwitterFilter TwitterFilter;
#else
typedef struct TwitterFilter TwitterFilter;
#endif /* __cplusplus */

#endif 	/* __TwitterFilter_FWD_DEFINED__ */


/* header files for imported files */
#include "oaidl.h"
#include "ocidl.h"
#include "filter.h"
#include "propsys.h"

#ifdef __cplusplus
extern "C"{
#endif 


/* interface __MIDL_itf_TwitterFullTextLib_0000_0000 */
/* [local] */ 

#pragma once
typedef struct tagTEXT_SOURCE
    {
    void *pfnFillTextBuffer;
    const WCHAR *awcBuffer;
    ULONG iEnd;
    ULONG iCur;
    } 	TEXT_SOURCE;

typedef 
enum tagWORDREP_BREAK_TYPE
    {
        WORDREP_BREAK_EOW	= 0,
        WORDREP_BREAK_EOS	= 1,
        WORDREP_BREAK_EOP	= 2,
        WORDREP_BREAK_EOC	= 3
    } 	WORDREP_BREAK_TYPE;



extern RPC_IF_HANDLE __MIDL_itf_TwitterFullTextLib_0000_0000_v0_0_c_ifspec;
extern RPC_IF_HANDLE __MIDL_itf_TwitterFullTextLib_0000_0000_v0_0_s_ifspec;

#ifndef __IWordSink_INTERFACE_DEFINED__
#define __IWordSink_INTERFACE_DEFINED__

/* interface IWordSink */
/* [local][object][uuid] */ 


EXTERN_C const IID IID_IWordSink;

#if defined(__cplusplus) && !defined(CINTERFACE)
    
    MIDL_INTERFACE("cc907054-c058-101a-b554-08002b33b0e6")
    IWordSink : public IUnknown
    {
    public:
        virtual HRESULT STDMETHODCALLTYPE PutWord( 
            /* [in] */ ULONG cwc,
            /* [in][size_is] */ const WCHAR *pwcInBuf,
            /* [in] */ ULONG cwcSrcLen,
            /* [in] */ ULONG cwcSrcPos) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE PutAltWord( 
            /* [in] */ ULONG cwc,
            /* [in][size_is] */ const WCHAR *pwcInBuf,
            /* [in] */ ULONG cwcSrcLen,
            /* [in] */ ULONG cwcSrcPos) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE StartAltPhrase( void) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE EndAltPhrase( void) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE PutBreak( 
            /* [in] */ WORDREP_BREAK_TYPE breakType) = 0;
        
    };
    
    
#else 	/* C style interface */

    typedef struct IWordSinkVtbl
    {
        BEGIN_INTERFACE
        
        HRESULT ( STDMETHODCALLTYPE *QueryInterface )( 
            IWordSink * This,
            /* [in] */ REFIID riid,
            /* [annotation][iid_is][out] */ 
            _COM_Outptr_  void **ppvObject);
        
        ULONG ( STDMETHODCALLTYPE *AddRef )( 
            IWordSink * This);
        
        ULONG ( STDMETHODCALLTYPE *Release )( 
            IWordSink * This);
        
        HRESULT ( STDMETHODCALLTYPE *PutWord )( 
            IWordSink * This,
            /* [in] */ ULONG cwc,
            /* [in][size_is] */ const WCHAR *pwcInBuf,
            /* [in] */ ULONG cwcSrcLen,
            /* [in] */ ULONG cwcSrcPos);
        
        HRESULT ( STDMETHODCALLTYPE *PutAltWord )( 
            IWordSink * This,
            /* [in] */ ULONG cwc,
            /* [in][size_is] */ const WCHAR *pwcInBuf,
            /* [in] */ ULONG cwcSrcLen,
            /* [in] */ ULONG cwcSrcPos);
        
        HRESULT ( STDMETHODCALLTYPE *StartAltPhrase )( 
            IWordSink * This);
        
        HRESULT ( STDMETHODCALLTYPE *EndAltPhrase )( 
            IWordSink * This);
        
        HRESULT ( STDMETHODCALLTYPE *PutBreak )( 
            IWordSink * This,
            /* [in] */ WORDREP_BREAK_TYPE breakType);
        
        END_INTERFACE
    } IWordSinkVtbl;

    interface IWordSink
    {
        CONST_VTBL struct IWordSinkVtbl *lpVtbl;
    };

    

#ifdef COBJMACROS


#define IWordSink_QueryInterface(This,riid,ppvObject)	\
    ( (This)->lpVtbl -> QueryInterface(This,riid,ppvObject) ) 

#define IWordSink_AddRef(This)	\
    ( (This)->lpVtbl -> AddRef(This) ) 

#define IWordSink_Release(This)	\
    ( (This)->lpVtbl -> Release(This) ) 


#define IWordSink_PutWord(This,cwc,pwcInBuf,cwcSrcLen,cwcSrcPos)	\
    ( (This)->lpVtbl -> PutWord(This,cwc,pwcInBuf,cwcSrcLen,cwcSrcPos) ) 

#define IWordSink_PutAltWord(This,cwc,pwcInBuf,cwcSrcLen,cwcSrcPos)	\
    ( (This)->lpVtbl -> PutAltWord(This,cwc,pwcInBuf,cwcSrcLen,cwcSrcPos) ) 

#define IWordSink_StartAltPhrase(This)	\
    ( (This)->lpVtbl -> StartAltPhrase(This) ) 

#define IWordSink_EndAltPhrase(This)	\
    ( (This)->lpVtbl -> EndAltPhrase(This) ) 

#define IWordSink_PutBreak(This,breakType)	\
    ( (This)->lpVtbl -> PutBreak(This,breakType) ) 

#endif /* COBJMACROS */


#endif 	/* C style interface */




#endif 	/* __IWordSink_INTERFACE_DEFINED__ */


#ifndef __IPhraseSink_INTERFACE_DEFINED__
#define __IPhraseSink_INTERFACE_DEFINED__

/* interface IPhraseSink */
/* [local][object][uuid] */ 


EXTERN_C const IID IID_IPhraseSink;

#if defined(__cplusplus) && !defined(CINTERFACE)
    
    MIDL_INTERFACE("cc906ff0-c058-101a-b554-08002b33b0e6")
    IPhraseSink : public IUnknown
    {
    public:
        virtual HRESULT STDMETHODCALLTYPE PutSmallPhrase( 
            /* [in][size_is] */ const WCHAR *pwcNoun,
            /* [in] */ ULONG cwcNoun,
            /* [in][size_is] */ const WCHAR *pwcModifier,
            /* [in] */ ULONG cwcModifier,
            /* [in] */ ULONG ulAttachmentType) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE PutPhrase( 
            /* [in][size_is] */ const WCHAR *pwcPhrase,
            /* [in] */ ULONG cwcPhrase) = 0;
        
    };
    
    
#else 	/* C style interface */

    typedef struct IPhraseSinkVtbl
    {
        BEGIN_INTERFACE
        
        HRESULT ( STDMETHODCALLTYPE *QueryInterface )( 
            IPhraseSink * This,
            /* [in] */ REFIID riid,
            /* [annotation][iid_is][out] */ 
            _COM_Outptr_  void **ppvObject);
        
        ULONG ( STDMETHODCALLTYPE *AddRef )( 
            IPhraseSink * This);
        
        ULONG ( STDMETHODCALLTYPE *Release )( 
            IPhraseSink * This);
        
        HRESULT ( STDMETHODCALLTYPE *PutSmallPhrase )( 
            IPhraseSink * This,
            /* [in][size_is] */ const WCHAR *pwcNoun,
            /* [in] */ ULONG cwcNoun,
            /* [in][size_is] */ const WCHAR *pwcModifier,
            /* [in] */ ULONG cwcModifier,
            /* [in] */ ULONG ulAttachmentType);
        
        HRESULT ( STDMETHODCALLTYPE *PutPhrase )( 
            IPhraseSink * This,
            /* [in][size_is] */ const WCHAR *pwcPhrase,
            /* [in] */ ULONG cwcPhrase);
        
        END_INTERFACE
    } IPhraseSinkVtbl;

    interface IPhraseSink
    {
        CONST_VTBL struct IPhraseSinkVtbl *lpVtbl;
    };

    

#ifdef COBJMACROS


#define IPhraseSink_QueryInterface(This,riid,ppvObject)	\
    ( (This)->lpVtbl -> QueryInterface(This,riid,ppvObject) ) 

#define IPhraseSink_AddRef(This)	\
    ( (This)->lpVtbl -> AddRef(This) ) 

#define IPhraseSink_Release(This)	\
    ( (This)->lpVtbl -> Release(This) ) 


#define IPhraseSink_PutSmallPhrase(This,pwcNoun,cwcNoun,pwcModifier,cwcModifier,ulAttachmentType)	\
    ( (This)->lpVtbl -> PutSmallPhrase(This,pwcNoun,cwcNoun,pwcModifier,cwcModifier,ulAttachmentType) ) 

#define IPhraseSink_PutPhrase(This,pwcPhrase,cwcPhrase)	\
    ( (This)->lpVtbl -> PutPhrase(This,pwcPhrase,cwcPhrase) ) 

#endif /* COBJMACROS */


#endif 	/* C style interface */




#endif 	/* __IPhraseSink_INTERFACE_DEFINED__ */


#ifndef __IWordBreaker_INTERFACE_DEFINED__
#define __IWordBreaker_INTERFACE_DEFINED__

/* interface IWordBreaker */
/* [local][object][uuid] */ 


EXTERN_C const IID IID_IWordBreaker;

#if defined(__cplusplus) && !defined(CINTERFACE)
    
    MIDL_INTERFACE("D53552C8-77E3-101A-B552-08002B33B0E6")
    IWordBreaker : public IUnknown
    {
    public:
        virtual HRESULT STDMETHODCALLTYPE Init( 
            /* [in] */ BOOL fQuery,
            /* [in] */ ULONG ulMaxTokenSize,
            /* [out] */ BOOL *pfLicense) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE BreakText( 
            /* [in] */ TEXT_SOURCE *pTextSource,
            /* [in] */ IWordSink *pWordSink,
            /* [in] */ IPhraseSink *pPhraseSink) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE ComposePhrase( 
            /* [in][size_is] */ const WCHAR *pwcNoun,
            /* [in] */ ULONG cwcNoun,
            /* [in][size_is] */ const WCHAR *pwcModifier,
            /* [in] */ ULONG cwcModifier,
            /* [in] */ ULONG ulAttachmentType,
            /* [out][size_is] */ WCHAR *pwcPhrase,
            /* [in][out] */ ULONG *pcwcPhrase) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE GetLicenseToUse( 
            /* [out][string] */ const WCHAR **ppwcsLicense) = 0;
        
    };
    
    
#else 	/* C style interface */

    typedef struct IWordBreakerVtbl
    {
        BEGIN_INTERFACE
        
        HRESULT ( STDMETHODCALLTYPE *QueryInterface )( 
            IWordBreaker * This,
            /* [in] */ REFIID riid,
            /* [annotation][iid_is][out] */ 
            _COM_Outptr_  void **ppvObject);
        
        ULONG ( STDMETHODCALLTYPE *AddRef )( 
            IWordBreaker * This);
        
        ULONG ( STDMETHODCALLTYPE *Release )( 
            IWordBreaker * This);
        
        HRESULT ( STDMETHODCALLTYPE *Init )( 
            IWordBreaker * This,
            /* [in] */ BOOL fQuery,
            /* [in] */ ULONG ulMaxTokenSize,
            /* [out] */ BOOL *pfLicense);
        
        HRESULT ( STDMETHODCALLTYPE *BreakText )( 
            IWordBreaker * This,
            /* [in] */ TEXT_SOURCE *pTextSource,
            /* [in] */ IWordSink *pWordSink,
            /* [in] */ IPhraseSink *pPhraseSink);
        
        HRESULT ( STDMETHODCALLTYPE *ComposePhrase )( 
            IWordBreaker * This,
            /* [in][size_is] */ const WCHAR *pwcNoun,
            /* [in] */ ULONG cwcNoun,
            /* [in][size_is] */ const WCHAR *pwcModifier,
            /* [in] */ ULONG cwcModifier,
            /* [in] */ ULONG ulAttachmentType,
            /* [out][size_is] */ WCHAR *pwcPhrase,
            /* [in][out] */ ULONG *pcwcPhrase);
        
        HRESULT ( STDMETHODCALLTYPE *GetLicenseToUse )( 
            IWordBreaker * This,
            /* [out][string] */ const WCHAR **ppwcsLicense);
        
        END_INTERFACE
    } IWordBreakerVtbl;

    interface IWordBreaker
    {
        CONST_VTBL struct IWordBreakerVtbl *lpVtbl;
    };

    

#ifdef COBJMACROS


#define IWordBreaker_QueryInterface(This,riid,ppvObject)	\
    ( (This)->lpVtbl -> QueryInterface(This,riid,ppvObject) ) 

#define IWordBreaker_AddRef(This)	\
    ( (This)->lpVtbl -> AddRef(This) ) 

#define IWordBreaker_Release(This)	\
    ( (This)->lpVtbl -> Release(This) ) 


#define IWordBreaker_Init(This,fQuery,ulMaxTokenSize,pfLicense)	\
    ( (This)->lpVtbl -> Init(This,fQuery,ulMaxTokenSize,pfLicense) ) 

#define IWordBreaker_BreakText(This,pTextSource,pWordSink,pPhraseSink)	\
    ( (This)->lpVtbl -> BreakText(This,pTextSource,pWordSink,pPhraseSink) ) 

#define IWordBreaker_ComposePhrase(This,pwcNoun,cwcNoun,pwcModifier,cwcModifier,ulAttachmentType,pwcPhrase,pcwcPhrase)	\
    ( (This)->lpVtbl -> ComposePhrase(This,pwcNoun,cwcNoun,pwcModifier,cwcModifier,ulAttachmentType,pwcPhrase,pcwcPhrase) ) 

#define IWordBreaker_GetLicenseToUse(This,ppwcsLicense)	\
    ( (This)->lpVtbl -> GetLicenseToUse(This,ppwcsLicense) ) 

#endif /* COBJMACROS */


#endif 	/* C style interface */




#endif 	/* __IWordBreaker_INTERFACE_DEFINED__ */


#ifndef __IWordFormSink_INTERFACE_DEFINED__
#define __IWordFormSink_INTERFACE_DEFINED__

/* interface IWordFormSink */
/* [local][object][uuid] */ 


EXTERN_C const IID IID_IWordFormSink;

#if defined(__cplusplus) && !defined(CINTERFACE)
    
    MIDL_INTERFACE("fe77c330-7f42-11ce-be57-00aa0051fe20")
    IWordFormSink : public IUnknown
    {
    public:
        virtual HRESULT STDMETHODCALLTYPE PutAltWord( 
            /* [in][size_is] */ const WCHAR *pwcInBuf,
            /* [in] */ ULONG cwc) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE PutWord( 
            /* [in][size_is] */ const WCHAR *pwcInBuf,
            /* [in] */ ULONG cwc) = 0;
        
    };
    
    
#else 	/* C style interface */

    typedef struct IWordFormSinkVtbl
    {
        BEGIN_INTERFACE
        
        HRESULT ( STDMETHODCALLTYPE *QueryInterface )( 
            IWordFormSink * This,
            /* [in] */ REFIID riid,
            /* [annotation][iid_is][out] */ 
            _COM_Outptr_  void **ppvObject);
        
        ULONG ( STDMETHODCALLTYPE *AddRef )( 
            IWordFormSink * This);
        
        ULONG ( STDMETHODCALLTYPE *Release )( 
            IWordFormSink * This);
        
        HRESULT ( STDMETHODCALLTYPE *PutAltWord )( 
            IWordFormSink * This,
            /* [in][size_is] */ const WCHAR *pwcInBuf,
            /* [in] */ ULONG cwc);
        
        HRESULT ( STDMETHODCALLTYPE *PutWord )( 
            IWordFormSink * This,
            /* [in][size_is] */ const WCHAR *pwcInBuf,
            /* [in] */ ULONG cwc);
        
        END_INTERFACE
    } IWordFormSinkVtbl;

    interface IWordFormSink
    {
        CONST_VTBL struct IWordFormSinkVtbl *lpVtbl;
    };

    

#ifdef COBJMACROS


#define IWordFormSink_QueryInterface(This,riid,ppvObject)	\
    ( (This)->lpVtbl -> QueryInterface(This,riid,ppvObject) ) 

#define IWordFormSink_AddRef(This)	\
    ( (This)->lpVtbl -> AddRef(This) ) 

#define IWordFormSink_Release(This)	\
    ( (This)->lpVtbl -> Release(This) ) 


#define IWordFormSink_PutAltWord(This,pwcInBuf,cwc)	\
    ( (This)->lpVtbl -> PutAltWord(This,pwcInBuf,cwc) ) 

#define IWordFormSink_PutWord(This,pwcInBuf,cwc)	\
    ( (This)->lpVtbl -> PutWord(This,pwcInBuf,cwc) ) 

#endif /* COBJMACROS */


#endif 	/* C style interface */




#endif 	/* __IWordFormSink_INTERFACE_DEFINED__ */


#ifndef __IStemmer_INTERFACE_DEFINED__
#define __IStemmer_INTERFACE_DEFINED__

/* interface IStemmer */
/* [local][object][uuid] */ 


EXTERN_C const IID IID_IStemmer;

#if defined(__cplusplus) && !defined(CINTERFACE)
    
    MIDL_INTERFACE("efbaf140-7f42-11ce-be57-00aa0051fe20")
    IStemmer : public IUnknown
    {
    public:
        virtual HRESULT STDMETHODCALLTYPE Init( 
            /* [in] */ ULONG ulMaxTokenSize,
            /* [out] */ BOOL *pfLicense) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE GenerateWordForms( 
            /* [in] */ const WCHAR *pwcInBuf,
            /* [in] */ ULONG cwc,
            /* [in] */ IWordFormSink *pStemSink) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE GetLicenseToUse( 
            /* [out][string] */ const WCHAR **ppwcsLicense) = 0;
        
    };
    
    
#else 	/* C style interface */

    typedef struct IStemmerVtbl
    {
        BEGIN_INTERFACE
        
        HRESULT ( STDMETHODCALLTYPE *QueryInterface )( 
            IStemmer * This,
            /* [in] */ REFIID riid,
            /* [annotation][iid_is][out] */ 
            _COM_Outptr_  void **ppvObject);
        
        ULONG ( STDMETHODCALLTYPE *AddRef )( 
            IStemmer * This);
        
        ULONG ( STDMETHODCALLTYPE *Release )( 
            IStemmer * This);
        
        HRESULT ( STDMETHODCALLTYPE *Init )( 
            IStemmer * This,
            /* [in] */ ULONG ulMaxTokenSize,
            /* [out] */ BOOL *pfLicense);
        
        HRESULT ( STDMETHODCALLTYPE *GenerateWordForms )( 
            IStemmer * This,
            /* [in] */ const WCHAR *pwcInBuf,
            /* [in] */ ULONG cwc,
            /* [in] */ IWordFormSink *pStemSink);
        
        HRESULT ( STDMETHODCALLTYPE *GetLicenseToUse )( 
            IStemmer * This,
            /* [out][string] */ const WCHAR **ppwcsLicense);
        
        END_INTERFACE
    } IStemmerVtbl;

    interface IStemmer
    {
        CONST_VTBL struct IStemmerVtbl *lpVtbl;
    };

    

#ifdef COBJMACROS


#define IStemmer_QueryInterface(This,riid,ppvObject)	\
    ( (This)->lpVtbl -> QueryInterface(This,riid,ppvObject) ) 

#define IStemmer_AddRef(This)	\
    ( (This)->lpVtbl -> AddRef(This) ) 

#define IStemmer_Release(This)	\
    ( (This)->lpVtbl -> Release(This) ) 


#define IStemmer_Init(This,ulMaxTokenSize,pfLicense)	\
    ( (This)->lpVtbl -> Init(This,ulMaxTokenSize,pfLicense) ) 

#define IStemmer_GenerateWordForms(This,pwcInBuf,cwc,pStemSink)	\
    ( (This)->lpVtbl -> GenerateWordForms(This,pwcInBuf,cwc,pStemSink) ) 

#define IStemmer_GetLicenseToUse(This,ppwcsLicense)	\
    ( (This)->lpVtbl -> GetLicenseToUse(This,ppwcsLicense) ) 

#endif /* COBJMACROS */


#endif 	/* C style interface */




#endif 	/* __IStemmer_INTERFACE_DEFINED__ */


#ifndef __ITwitterWordBreaker_INTERFACE_DEFINED__
#define __ITwitterWordBreaker_INTERFACE_DEFINED__

/* interface ITwitterWordBreaker */
/* [unique][nonextensible][dual][uuid][object] */ 


EXTERN_C const IID IID_ITwitterWordBreaker;

#if defined(__cplusplus) && !defined(CINTERFACE)
    
    MIDL_INTERFACE("50020E05-9231-4B79-8C75-DAEF6064A9D9")
    ITwitterWordBreaker : public IDispatch
    {
    public:
    };
    
    
#else 	/* C style interface */

    typedef struct ITwitterWordBreakerVtbl
    {
        BEGIN_INTERFACE
        
        HRESULT ( STDMETHODCALLTYPE *QueryInterface )( 
            ITwitterWordBreaker * This,
            /* [in] */ REFIID riid,
            /* [annotation][iid_is][out] */ 
            _COM_Outptr_  void **ppvObject);
        
        ULONG ( STDMETHODCALLTYPE *AddRef )( 
            ITwitterWordBreaker * This);
        
        ULONG ( STDMETHODCALLTYPE *Release )( 
            ITwitterWordBreaker * This);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfoCount )( 
            ITwitterWordBreaker * This,
            /* [out] */ UINT *pctinfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfo )( 
            ITwitterWordBreaker * This,
            /* [in] */ UINT iTInfo,
            /* [in] */ LCID lcid,
            /* [out] */ ITypeInfo **ppTInfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetIDsOfNames )( 
            ITwitterWordBreaker * This,
            /* [in] */ REFIID riid,
            /* [size_is][in] */ LPOLESTR *rgszNames,
            /* [range][in] */ UINT cNames,
            /* [in] */ LCID lcid,
            /* [size_is][out] */ DISPID *rgDispId);
        
        /* [local] */ HRESULT ( STDMETHODCALLTYPE *Invoke )( 
            ITwitterWordBreaker * This,
            /* [annotation][in] */ 
            _In_  DISPID dispIdMember,
            /* [annotation][in] */ 
            _In_  REFIID riid,
            /* [annotation][in] */ 
            _In_  LCID lcid,
            /* [annotation][in] */ 
            _In_  WORD wFlags,
            /* [annotation][out][in] */ 
            _In_  DISPPARAMS *pDispParams,
            /* [annotation][out] */ 
            _Out_opt_  VARIANT *pVarResult,
            /* [annotation][out] */ 
            _Out_opt_  EXCEPINFO *pExcepInfo,
            /* [annotation][out] */ 
            _Out_opt_  UINT *puArgErr);
        
        END_INTERFACE
    } ITwitterWordBreakerVtbl;

    interface ITwitterWordBreaker
    {
        CONST_VTBL struct ITwitterWordBreakerVtbl *lpVtbl;
    };

    

#ifdef COBJMACROS


#define ITwitterWordBreaker_QueryInterface(This,riid,ppvObject)	\
    ( (This)->lpVtbl -> QueryInterface(This,riid,ppvObject) ) 

#define ITwitterWordBreaker_AddRef(This)	\
    ( (This)->lpVtbl -> AddRef(This) ) 

#define ITwitterWordBreaker_Release(This)	\
    ( (This)->lpVtbl -> Release(This) ) 


#define ITwitterWordBreaker_GetTypeInfoCount(This,pctinfo)	\
    ( (This)->lpVtbl -> GetTypeInfoCount(This,pctinfo) ) 

#define ITwitterWordBreaker_GetTypeInfo(This,iTInfo,lcid,ppTInfo)	\
    ( (This)->lpVtbl -> GetTypeInfo(This,iTInfo,lcid,ppTInfo) ) 

#define ITwitterWordBreaker_GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId)	\
    ( (This)->lpVtbl -> GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId) ) 

#define ITwitterWordBreaker_Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr)	\
    ( (This)->lpVtbl -> Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr) ) 


#endif /* COBJMACROS */


#endif 	/* C style interface */




#endif 	/* __ITwitterWordBreaker_INTERFACE_DEFINED__ */


#ifndef __ITwitterStemmer_INTERFACE_DEFINED__
#define __ITwitterStemmer_INTERFACE_DEFINED__

/* interface ITwitterStemmer */
/* [unique][nonextensible][dual][uuid][object] */ 


EXTERN_C const IID IID_ITwitterStemmer;

#if defined(__cplusplus) && !defined(CINTERFACE)
    
    MIDL_INTERFACE("43FF105D-2E61-4161-A7DB-AD084058313E")
    ITwitterStemmer : public IDispatch
    {
    public:
    };
    
    
#else 	/* C style interface */

    typedef struct ITwitterStemmerVtbl
    {
        BEGIN_INTERFACE
        
        HRESULT ( STDMETHODCALLTYPE *QueryInterface )( 
            ITwitterStemmer * This,
            /* [in] */ REFIID riid,
            /* [annotation][iid_is][out] */ 
            _COM_Outptr_  void **ppvObject);
        
        ULONG ( STDMETHODCALLTYPE *AddRef )( 
            ITwitterStemmer * This);
        
        ULONG ( STDMETHODCALLTYPE *Release )( 
            ITwitterStemmer * This);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfoCount )( 
            ITwitterStemmer * This,
            /* [out] */ UINT *pctinfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfo )( 
            ITwitterStemmer * This,
            /* [in] */ UINT iTInfo,
            /* [in] */ LCID lcid,
            /* [out] */ ITypeInfo **ppTInfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetIDsOfNames )( 
            ITwitterStemmer * This,
            /* [in] */ REFIID riid,
            /* [size_is][in] */ LPOLESTR *rgszNames,
            /* [range][in] */ UINT cNames,
            /* [in] */ LCID lcid,
            /* [size_is][out] */ DISPID *rgDispId);
        
        /* [local] */ HRESULT ( STDMETHODCALLTYPE *Invoke )( 
            ITwitterStemmer * This,
            /* [annotation][in] */ 
            _In_  DISPID dispIdMember,
            /* [annotation][in] */ 
            _In_  REFIID riid,
            /* [annotation][in] */ 
            _In_  LCID lcid,
            /* [annotation][in] */ 
            _In_  WORD wFlags,
            /* [annotation][out][in] */ 
            _In_  DISPPARAMS *pDispParams,
            /* [annotation][out] */ 
            _Out_opt_  VARIANT *pVarResult,
            /* [annotation][out] */ 
            _Out_opt_  EXCEPINFO *pExcepInfo,
            /* [annotation][out] */ 
            _Out_opt_  UINT *puArgErr);
        
        END_INTERFACE
    } ITwitterStemmerVtbl;

    interface ITwitterStemmer
    {
        CONST_VTBL struct ITwitterStemmerVtbl *lpVtbl;
    };

    

#ifdef COBJMACROS


#define ITwitterStemmer_QueryInterface(This,riid,ppvObject)	\
    ( (This)->lpVtbl -> QueryInterface(This,riid,ppvObject) ) 

#define ITwitterStemmer_AddRef(This)	\
    ( (This)->lpVtbl -> AddRef(This) ) 

#define ITwitterStemmer_Release(This)	\
    ( (This)->lpVtbl -> Release(This) ) 


#define ITwitterStemmer_GetTypeInfoCount(This,pctinfo)	\
    ( (This)->lpVtbl -> GetTypeInfoCount(This,pctinfo) ) 

#define ITwitterStemmer_GetTypeInfo(This,iTInfo,lcid,ppTInfo)	\
    ( (This)->lpVtbl -> GetTypeInfo(This,iTInfo,lcid,ppTInfo) ) 

#define ITwitterStemmer_GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId)	\
    ( (This)->lpVtbl -> GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId) ) 

#define ITwitterStemmer_Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr)	\
    ( (This)->lpVtbl -> Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr) ) 


#endif /* COBJMACROS */


#endif 	/* C style interface */




#endif 	/* __ITwitterStemmer_INTERFACE_DEFINED__ */


#ifndef __ITwitterFilter_INTERFACE_DEFINED__
#define __ITwitterFilter_INTERFACE_DEFINED__

/* interface ITwitterFilter */
/* [unique][nonextensible][dual][uuid][object] */ 


EXTERN_C const IID IID_ITwitterFilter;

#if defined(__cplusplus) && !defined(CINTERFACE)
    
    MIDL_INTERFACE("A04A44DD-079A-462B-A75F-03129D6A5996")
    ITwitterFilter : public IDispatch
    {
    public:
    };
    
    
#else 	/* C style interface */

    typedef struct ITwitterFilterVtbl
    {
        BEGIN_INTERFACE
        
        HRESULT ( STDMETHODCALLTYPE *QueryInterface )( 
            ITwitterFilter * This,
            /* [in] */ REFIID riid,
            /* [annotation][iid_is][out] */ 
            _COM_Outptr_  void **ppvObject);
        
        ULONG ( STDMETHODCALLTYPE *AddRef )( 
            ITwitterFilter * This);
        
        ULONG ( STDMETHODCALLTYPE *Release )( 
            ITwitterFilter * This);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfoCount )( 
            ITwitterFilter * This,
            /* [out] */ UINT *pctinfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfo )( 
            ITwitterFilter * This,
            /* [in] */ UINT iTInfo,
            /* [in] */ LCID lcid,
            /* [out] */ ITypeInfo **ppTInfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetIDsOfNames )( 
            ITwitterFilter * This,
            /* [in] */ REFIID riid,
            /* [size_is][in] */ LPOLESTR *rgszNames,
            /* [range][in] */ UINT cNames,
            /* [in] */ LCID lcid,
            /* [size_is][out] */ DISPID *rgDispId);
        
        /* [local] */ HRESULT ( STDMETHODCALLTYPE *Invoke )( 
            ITwitterFilter * This,
            /* [annotation][in] */ 
            _In_  DISPID dispIdMember,
            /* [annotation][in] */ 
            _In_  REFIID riid,
            /* [annotation][in] */ 
            _In_  LCID lcid,
            /* [annotation][in] */ 
            _In_  WORD wFlags,
            /* [annotation][out][in] */ 
            _In_  DISPPARAMS *pDispParams,
            /* [annotation][out] */ 
            _Out_opt_  VARIANT *pVarResult,
            /* [annotation][out] */ 
            _Out_opt_  EXCEPINFO *pExcepInfo,
            /* [annotation][out] */ 
            _Out_opt_  UINT *puArgErr);
        
        END_INTERFACE
    } ITwitterFilterVtbl;

    interface ITwitterFilter
    {
        CONST_VTBL struct ITwitterFilterVtbl *lpVtbl;
    };

    

#ifdef COBJMACROS


#define ITwitterFilter_QueryInterface(This,riid,ppvObject)	\
    ( (This)->lpVtbl -> QueryInterface(This,riid,ppvObject) ) 

#define ITwitterFilter_AddRef(This)	\
    ( (This)->lpVtbl -> AddRef(This) ) 

#define ITwitterFilter_Release(This)	\
    ( (This)->lpVtbl -> Release(This) ) 


#define ITwitterFilter_GetTypeInfoCount(This,pctinfo)	\
    ( (This)->lpVtbl -> GetTypeInfoCount(This,pctinfo) ) 

#define ITwitterFilter_GetTypeInfo(This,iTInfo,lcid,ppTInfo)	\
    ( (This)->lpVtbl -> GetTypeInfo(This,iTInfo,lcid,ppTInfo) ) 

#define ITwitterFilter_GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId)	\
    ( (This)->lpVtbl -> GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId) ) 

#define ITwitterFilter_Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr)	\
    ( (This)->lpVtbl -> Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr) ) 


#endif /* COBJMACROS */


#endif 	/* C style interface */




#endif 	/* __ITwitterFilter_INTERFACE_DEFINED__ */



#ifndef __TwitterFullTextLib_LIBRARY_DEFINED__
#define __TwitterFullTextLib_LIBRARY_DEFINED__

/* library TwitterFullTextLib */
/* [version][uuid] */ 


EXTERN_C const IID LIBID_TwitterFullTextLib;

EXTERN_C const CLSID CLSID_TwitterWordBreaker;

#ifdef __cplusplus

class DECLSPEC_UUID("C9A48231-51C4-439B-AFF3-C4BE123EEB86")
TwitterWordBreaker;
#endif

EXTERN_C const CLSID CLSID_TwitterStemmer;

#ifdef __cplusplus

class DECLSPEC_UUID("07CB0BC6-A4C4-4E92-BE56-579F333F6CB3")
TwitterStemmer;
#endif

EXTERN_C const CLSID CLSID_TwitterFilter;

#ifdef __cplusplus

class DECLSPEC_UUID("64BB8B78-12F3-42DE-8BCF-229C24DDB3E0")
TwitterFilter;
#endif
#endif /* __TwitterFullTextLib_LIBRARY_DEFINED__ */

/* Additional Prototypes for ALL interfaces */

/* end of Additional Prototypes */

#ifdef __cplusplus
}
#endif

#endif


