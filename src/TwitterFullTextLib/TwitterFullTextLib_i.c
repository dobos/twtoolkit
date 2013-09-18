

/* this ALWAYS GENERATED file contains the IIDs and CLSIDs */

/* link this file in with the server and any clients */


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

#pragma warning( disable: 4049 )  /* more than 64k source lines */


#ifdef __cplusplus
extern "C"{
#endif 


#include <rpc.h>
#include <rpcndr.h>

#ifdef _MIDL_USE_GUIDDEF_

#ifndef INITGUID
#define INITGUID
#include <guiddef.h>
#undef INITGUID
#else
#include <guiddef.h>
#endif

#define MIDL_DEFINE_GUID(type,name,l,w1,w2,b1,b2,b3,b4,b5,b6,b7,b8) \
        DEFINE_GUID(name,l,w1,w2,b1,b2,b3,b4,b5,b6,b7,b8)

#else // !_MIDL_USE_GUIDDEF_

#ifndef __IID_DEFINED__
#define __IID_DEFINED__

typedef struct _IID
{
    unsigned long x;
    unsigned short s1;
    unsigned short s2;
    unsigned char  c[8];
} IID;

#endif // __IID_DEFINED__

#ifndef CLSID_DEFINED
#define CLSID_DEFINED
typedef IID CLSID;
#endif // CLSID_DEFINED

#define MIDL_DEFINE_GUID(type,name,l,w1,w2,b1,b2,b3,b4,b5,b6,b7,b8) \
        const type name = {l,w1,w2,{b1,b2,b3,b4,b5,b6,b7,b8}}

#endif !_MIDL_USE_GUIDDEF_

MIDL_DEFINE_GUID(IID, IID_IWordSink,0xcc907054,0xc058,0x101a,0xb5,0x54,0x08,0x00,0x2b,0x33,0xb0,0xe6);


MIDL_DEFINE_GUID(IID, IID_IPhraseSink,0xcc906ff0,0xc058,0x101a,0xb5,0x54,0x08,0x00,0x2b,0x33,0xb0,0xe6);


MIDL_DEFINE_GUID(IID, IID_IWordBreaker,0xD53552C8,0x77E3,0x101A,0xB5,0x52,0x08,0x00,0x2B,0x33,0xB0,0xE6);


MIDL_DEFINE_GUID(IID, IID_IWordFormSink,0xfe77c330,0x7f42,0x11ce,0xbe,0x57,0x00,0xaa,0x00,0x51,0xfe,0x20);


MIDL_DEFINE_GUID(IID, IID_IStemmer,0xefbaf140,0x7f42,0x11ce,0xbe,0x57,0x00,0xaa,0x00,0x51,0xfe,0x20);


MIDL_DEFINE_GUID(IID, IID_ITwitterWordBreaker,0x50020E05,0x9231,0x4B79,0x8C,0x75,0xDA,0xEF,0x60,0x64,0xA9,0xD9);


MIDL_DEFINE_GUID(IID, IID_ITwitterStemmer,0x43FF105D,0x2E61,0x4161,0xA7,0xDB,0xAD,0x08,0x40,0x58,0x31,0x3E);


MIDL_DEFINE_GUID(IID, IID_ITwitterFilter,0xA04A44DD,0x079A,0x462B,0xA7,0x5F,0x03,0x12,0x9D,0x6A,0x59,0x96);


MIDL_DEFINE_GUID(IID, LIBID_TwitterFullTextLib,0xB630B9A2,0xBE59,0x40EA,0xB2,0x6A,0x0B,0x75,0xFE,0x4B,0x21,0x63);


MIDL_DEFINE_GUID(CLSID, CLSID_TwitterWordBreaker,0xC9A48231,0x51C4,0x439B,0xAF,0xF3,0xC4,0xBE,0x12,0x3E,0xEB,0x86);


MIDL_DEFINE_GUID(CLSID, CLSID_TwitterStemmer,0x07CB0BC6,0xA4C4,0x4E92,0xBE,0x56,0x57,0x9F,0x33,0x3F,0x6C,0xB3);


MIDL_DEFINE_GUID(CLSID, CLSID_TwitterFilter,0x64BB8B78,0x12F3,0x42DE,0x8B,0xCF,0x22,0x9C,0x24,0xDD,0xB3,0xE0);

#undef MIDL_DEFINE_GUID

#ifdef __cplusplus
}
#endif



