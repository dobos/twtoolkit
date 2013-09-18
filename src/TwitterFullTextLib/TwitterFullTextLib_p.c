

/* this ALWAYS GENERATED file contains the proxy stub code */


 /* File created by MIDL compiler version 7.00.0555 */
/* at Wed Sep 18 21:07:06 2013
 */
/* Compiler settings for TwitterFullTextLib.idl:
    Oicf, W1, Zp8, env=Win32 (32b run), target_arch=X86 7.00.0555 
    protocol : dce , ms_ext, c_ext, robust
    error checks: allocation ref bounds_check enum stub_data 
    VC __declspec() decoration level: 
         __declspec(uuid()), __declspec(selectany), __declspec(novtable)
         DECLSPEC_UUID(), MIDL_INTERFACE()
*/
/* @@MIDL_FILE_HEADING(  ) */

#if !defined(_M_IA64) && !defined(_M_AMD64)


#pragma warning( disable: 4049 )  /* more than 64k source lines */
#if _MSC_VER >= 1200
#pragma warning(push)
#endif

#pragma warning( disable: 4211 )  /* redefine extern to static */
#pragma warning( disable: 4232 )  /* dllimport identity*/
#pragma warning( disable: 4024 )  /* array to pointer mapping*/
#pragma warning( disable: 4152 )  /* function/data pointer conversion in expression */
#pragma warning( disable: 4100 ) /* unreferenced arguments in x86 call */

#pragma optimize("", off ) 

#define USE_STUBLESS_PROXY


/* verify that the <rpcproxy.h> version is high enough to compile this file*/
#ifndef __REDQ_RPCPROXY_H_VERSION__
#define __REQUIRED_RPCPROXY_H_VERSION__ 475
#endif


#include "rpcproxy.h"
#ifndef __RPCPROXY_H_VERSION__
#error this stub requires an updated version of <rpcproxy.h>
#endif /* __RPCPROXY_H_VERSION__ */


#include "TwitterFullTextLib_i.h"

#define TYPE_FORMAT_STRING_SIZE   3                                 
#define PROC_FORMAT_STRING_SIZE   1                                 
#define EXPR_FORMAT_STRING_SIZE   1                                 
#define TRANSMIT_AS_TABLE_SIZE    0            
#define WIRE_MARSHAL_TABLE_SIZE   0            

typedef struct _TwitterFullTextLib_MIDL_TYPE_FORMAT_STRING
    {
    short          Pad;
    unsigned char  Format[ TYPE_FORMAT_STRING_SIZE ];
    } TwitterFullTextLib_MIDL_TYPE_FORMAT_STRING;

typedef struct _TwitterFullTextLib_MIDL_PROC_FORMAT_STRING
    {
    short          Pad;
    unsigned char  Format[ PROC_FORMAT_STRING_SIZE ];
    } TwitterFullTextLib_MIDL_PROC_FORMAT_STRING;

typedef struct _TwitterFullTextLib_MIDL_EXPR_FORMAT_STRING
    {
    long          Pad;
    unsigned char  Format[ EXPR_FORMAT_STRING_SIZE ];
    } TwitterFullTextLib_MIDL_EXPR_FORMAT_STRING;


static const RPC_SYNTAX_IDENTIFIER  _RpcTransferSyntax = 
{{0x8A885D04,0x1CEB,0x11C9,{0x9F,0xE8,0x08,0x00,0x2B,0x10,0x48,0x60}},{2,0}};


extern const TwitterFullTextLib_MIDL_TYPE_FORMAT_STRING TwitterFullTextLib__MIDL_TypeFormatString;
extern const TwitterFullTextLib_MIDL_PROC_FORMAT_STRING TwitterFullTextLib__MIDL_ProcFormatString;
extern const TwitterFullTextLib_MIDL_EXPR_FORMAT_STRING TwitterFullTextLib__MIDL_ExprFormatString;


extern const MIDL_STUB_DESC Object_StubDesc;


extern const MIDL_SERVER_INFO ITwitterWordBreaker_ServerInfo;
extern const MIDL_STUBLESS_PROXY_INFO ITwitterWordBreaker_ProxyInfo;


extern const MIDL_STUB_DESC Object_StubDesc;


extern const MIDL_SERVER_INFO ITwitterStemmer_ServerInfo;
extern const MIDL_STUBLESS_PROXY_INFO ITwitterStemmer_ProxyInfo;


extern const MIDL_STUB_DESC Object_StubDesc;


extern const MIDL_SERVER_INFO ITwitterFilter_ServerInfo;
extern const MIDL_STUBLESS_PROXY_INFO ITwitterFilter_ProxyInfo;



#if !defined(__RPC_WIN32__)
#error  Invalid build platform for this stub.
#endif

#if !(TARGET_IS_NT50_OR_LATER)
#error You need Windows 2000 or later to run this stub because it uses these features:
#error   /robust command line switch.
#error However, your C/C++ compilation flags indicate you intend to run this app on earlier systems.
#error This app will fail with the RPC_X_WRONG_STUB_VERSION error.
#endif


static const TwitterFullTextLib_MIDL_PROC_FORMAT_STRING TwitterFullTextLib__MIDL_ProcFormatString =
    {
        0,
        {

			0x0
        }
    };

static const TwitterFullTextLib_MIDL_TYPE_FORMAT_STRING TwitterFullTextLib__MIDL_TypeFormatString =
    {
        0,
        {
			NdrFcShort( 0x0 ),	/* 0 */

			0x0
        }
    };


/* Standard interface: __MIDL_itf_TwitterFullTextLib_0000_0000, ver. 0.0,
   GUID={0x00000000,0x0000,0x0000,{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00}} */


/* Object interface: IUnknown, ver. 0.0,
   GUID={0x00000000,0x0000,0x0000,{0xC0,0x00,0x00,0x00,0x00,0x00,0x00,0x46}} */


/* Object interface: IWordSink, ver. 0.0,
   GUID={0xcc907054,0xc058,0x101a,{0xb5,0x54,0x08,0x00,0x2b,0x33,0xb0,0xe6}} */


/* Object interface: IPhraseSink, ver. 0.0,
   GUID={0xcc906ff0,0xc058,0x101a,{0xb5,0x54,0x08,0x00,0x2b,0x33,0xb0,0xe6}} */


/* Object interface: IWordBreaker, ver. 0.0,
   GUID={0xD53552C8,0x77E3,0x101A,{0xB5,0x52,0x08,0x00,0x2B,0x33,0xB0,0xE6}} */


/* Object interface: IWordFormSink, ver. 0.0,
   GUID={0xfe77c330,0x7f42,0x11ce,{0xbe,0x57,0x00,0xaa,0x00,0x51,0xfe,0x20}} */


/* Object interface: IStemmer, ver. 0.0,
   GUID={0xefbaf140,0x7f42,0x11ce,{0xbe,0x57,0x00,0xaa,0x00,0x51,0xfe,0x20}} */


/* Object interface: IDispatch, ver. 0.0,
   GUID={0x00020400,0x0000,0x0000,{0xC0,0x00,0x00,0x00,0x00,0x00,0x00,0x46}} */


/* Object interface: ITwitterWordBreaker, ver. 0.0,
   GUID={0x50020E05,0x9231,0x4B79,{0x8C,0x75,0xDA,0xEF,0x60,0x64,0xA9,0xD9}} */

#pragma code_seg(".orpc")
static const unsigned short ITwitterWordBreaker_FormatStringOffsetTable[] =
    {
    (unsigned short) -1,
    (unsigned short) -1,
    (unsigned short) -1,
    (unsigned short) -1,
    0
    };

static const MIDL_STUBLESS_PROXY_INFO ITwitterWordBreaker_ProxyInfo =
    {
    &Object_StubDesc,
    TwitterFullTextLib__MIDL_ProcFormatString.Format,
    &ITwitterWordBreaker_FormatStringOffsetTable[-3],
    0,
    0,
    0
    };


static const MIDL_SERVER_INFO ITwitterWordBreaker_ServerInfo = 
    {
    &Object_StubDesc,
    0,
    TwitterFullTextLib__MIDL_ProcFormatString.Format,
    &ITwitterWordBreaker_FormatStringOffsetTable[-3],
    0,
    0,
    0,
    0};
CINTERFACE_PROXY_VTABLE(7) _ITwitterWordBreakerProxyVtbl = 
{
    0,
    &IID_ITwitterWordBreaker,
    IUnknown_QueryInterface_Proxy,
    IUnknown_AddRef_Proxy,
    IUnknown_Release_Proxy ,
    0 /* IDispatch::GetTypeInfoCount */ ,
    0 /* IDispatch::GetTypeInfo */ ,
    0 /* IDispatch::GetIDsOfNames */ ,
    0 /* IDispatch_Invoke_Proxy */
};


static const PRPC_STUB_FUNCTION ITwitterWordBreaker_table[] =
{
    STUB_FORWARDING_FUNCTION,
    STUB_FORWARDING_FUNCTION,
    STUB_FORWARDING_FUNCTION,
    STUB_FORWARDING_FUNCTION
};

CInterfaceStubVtbl _ITwitterWordBreakerStubVtbl =
{
    &IID_ITwitterWordBreaker,
    &ITwitterWordBreaker_ServerInfo,
    7,
    &ITwitterWordBreaker_table[-3],
    CStdStubBuffer_DELEGATING_METHODS
};


/* Object interface: ITwitterStemmer, ver. 0.0,
   GUID={0x43FF105D,0x2E61,0x4161,{0xA7,0xDB,0xAD,0x08,0x40,0x58,0x31,0x3E}} */

#pragma code_seg(".orpc")
static const unsigned short ITwitterStemmer_FormatStringOffsetTable[] =
    {
    (unsigned short) -1,
    (unsigned short) -1,
    (unsigned short) -1,
    (unsigned short) -1,
    0
    };

static const MIDL_STUBLESS_PROXY_INFO ITwitterStemmer_ProxyInfo =
    {
    &Object_StubDesc,
    TwitterFullTextLib__MIDL_ProcFormatString.Format,
    &ITwitterStemmer_FormatStringOffsetTable[-3],
    0,
    0,
    0
    };


static const MIDL_SERVER_INFO ITwitterStemmer_ServerInfo = 
    {
    &Object_StubDesc,
    0,
    TwitterFullTextLib__MIDL_ProcFormatString.Format,
    &ITwitterStemmer_FormatStringOffsetTable[-3],
    0,
    0,
    0,
    0};
CINTERFACE_PROXY_VTABLE(7) _ITwitterStemmerProxyVtbl = 
{
    0,
    &IID_ITwitterStemmer,
    IUnknown_QueryInterface_Proxy,
    IUnknown_AddRef_Proxy,
    IUnknown_Release_Proxy ,
    0 /* IDispatch::GetTypeInfoCount */ ,
    0 /* IDispatch::GetTypeInfo */ ,
    0 /* IDispatch::GetIDsOfNames */ ,
    0 /* IDispatch_Invoke_Proxy */
};


static const PRPC_STUB_FUNCTION ITwitterStemmer_table[] =
{
    STUB_FORWARDING_FUNCTION,
    STUB_FORWARDING_FUNCTION,
    STUB_FORWARDING_FUNCTION,
    STUB_FORWARDING_FUNCTION
};

CInterfaceStubVtbl _ITwitterStemmerStubVtbl =
{
    &IID_ITwitterStemmer,
    &ITwitterStemmer_ServerInfo,
    7,
    &ITwitterStemmer_table[-3],
    CStdStubBuffer_DELEGATING_METHODS
};


/* Object interface: ITwitterFilter, ver. 0.0,
   GUID={0xA04A44DD,0x079A,0x462B,{0xA7,0x5F,0x03,0x12,0x9D,0x6A,0x59,0x96}} */

#pragma code_seg(".orpc")
static const unsigned short ITwitterFilter_FormatStringOffsetTable[] =
    {
    (unsigned short) -1,
    (unsigned short) -1,
    (unsigned short) -1,
    (unsigned short) -1,
    0
    };

static const MIDL_STUBLESS_PROXY_INFO ITwitterFilter_ProxyInfo =
    {
    &Object_StubDesc,
    TwitterFullTextLib__MIDL_ProcFormatString.Format,
    &ITwitterFilter_FormatStringOffsetTable[-3],
    0,
    0,
    0
    };


static const MIDL_SERVER_INFO ITwitterFilter_ServerInfo = 
    {
    &Object_StubDesc,
    0,
    TwitterFullTextLib__MIDL_ProcFormatString.Format,
    &ITwitterFilter_FormatStringOffsetTable[-3],
    0,
    0,
    0,
    0};
CINTERFACE_PROXY_VTABLE(7) _ITwitterFilterProxyVtbl = 
{
    0,
    &IID_ITwitterFilter,
    IUnknown_QueryInterface_Proxy,
    IUnknown_AddRef_Proxy,
    IUnknown_Release_Proxy ,
    0 /* IDispatch::GetTypeInfoCount */ ,
    0 /* IDispatch::GetTypeInfo */ ,
    0 /* IDispatch::GetIDsOfNames */ ,
    0 /* IDispatch_Invoke_Proxy */
};


static const PRPC_STUB_FUNCTION ITwitterFilter_table[] =
{
    STUB_FORWARDING_FUNCTION,
    STUB_FORWARDING_FUNCTION,
    STUB_FORWARDING_FUNCTION,
    STUB_FORWARDING_FUNCTION
};

CInterfaceStubVtbl _ITwitterFilterStubVtbl =
{
    &IID_ITwitterFilter,
    &ITwitterFilter_ServerInfo,
    7,
    &ITwitterFilter_table[-3],
    CStdStubBuffer_DELEGATING_METHODS
};

static const MIDL_STUB_DESC Object_StubDesc = 
    {
    0,
    NdrOleAllocate,
    NdrOleFree,
    0,
    0,
    0,
    0,
    0,
    TwitterFullTextLib__MIDL_TypeFormatString.Format,
    1, /* -error bounds_check flag */
    0x50002, /* Ndr library version */
    0,
    0x700022b, /* MIDL Version 7.0.555 */
    0,
    0,
    0,  /* notify & notify_flag routine table */
    0x1, /* MIDL flag */
    0, /* cs routines */
    0,   /* proxy/server info */
    0
    };

const CInterfaceProxyVtbl * const _TwitterFullTextLib_ProxyVtblList[] = 
{
    ( CInterfaceProxyVtbl *) &_ITwitterWordBreakerProxyVtbl,
    ( CInterfaceProxyVtbl *) &_ITwitterStemmerProxyVtbl,
    ( CInterfaceProxyVtbl *) &_ITwitterFilterProxyVtbl,
    0
};

const CInterfaceStubVtbl * const _TwitterFullTextLib_StubVtblList[] = 
{
    ( CInterfaceStubVtbl *) &_ITwitterWordBreakerStubVtbl,
    ( CInterfaceStubVtbl *) &_ITwitterStemmerStubVtbl,
    ( CInterfaceStubVtbl *) &_ITwitterFilterStubVtbl,
    0
};

PCInterfaceName const _TwitterFullTextLib_InterfaceNamesList[] = 
{
    "ITwitterWordBreaker",
    "ITwitterStemmer",
    "ITwitterFilter",
    0
};

const IID *  const _TwitterFullTextLib_BaseIIDList[] = 
{
    &IID_IDispatch,
    &IID_IDispatch,
    &IID_IDispatch,
    0
};


#define _TwitterFullTextLib_CHECK_IID(n)	IID_GENERIC_CHECK_IID( _TwitterFullTextLib, pIID, n)

int __stdcall _TwitterFullTextLib_IID_Lookup( const IID * pIID, int * pIndex )
{
    IID_BS_LOOKUP_SETUP

    IID_BS_LOOKUP_INITIAL_TEST( _TwitterFullTextLib, 3, 2 )
    IID_BS_LOOKUP_NEXT_TEST( _TwitterFullTextLib, 1 )
    IID_BS_LOOKUP_RETURN_RESULT( _TwitterFullTextLib, 3, *pIndex )
    
}

const ExtendedProxyFileInfo TwitterFullTextLib_ProxyFileInfo = 
{
    (PCInterfaceProxyVtblList *) & _TwitterFullTextLib_ProxyVtblList,
    (PCInterfaceStubVtblList *) & _TwitterFullTextLib_StubVtblList,
    (const PCInterfaceName * ) & _TwitterFullTextLib_InterfaceNamesList,
    (const IID ** ) & _TwitterFullTextLib_BaseIIDList,
    & _TwitterFullTextLib_IID_Lookup, 
    3,
    2,
    0, /* table of [async_uuid] interfaces */
    0, /* Filler1 */
    0, /* Filler2 */
    0  /* Filler3 */
};
#pragma optimize("", on )
#if _MSC_VER >= 1200
#pragma warning(pop)
#endif


#endif /* !defined(_M_IA64) && !defined(_M_AMD64)*/

