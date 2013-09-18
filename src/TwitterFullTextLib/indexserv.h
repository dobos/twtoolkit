#pragma once

typedef struct tagTEXT_SOURCE
{
    void* pfnFillTextBuffer;
    const WCHAR *awcBuffer;
    ULONG iEnd;
    ULONG iCur;
} TEXT_SOURCE;

typedef enum tagWORDREP_BREAK_TYPE
{
    WORDREP_BREAK_EOW = 0,
    WORDREP_BREAK_EOS = 1,
    WORDREP_BREAK_EOP = 2,
    WORDREP_BREAK_EOC = 3
} WORDREP_BREAK_TYPE;

/*****************************************************************************
 *    IWordSink interface
 */
[
    uuid(cc907054-c058-101a-b554-08002b33b0e6),
    object,
    local
]
interface IWordSink : IUnknown
{
    HRESULT PutWord(
              [in] ULONG cwc,
              [size_is(cwcSrcLen)][in] const WCHAR *pwcInBuf,
              [in] ULONG cwcSrcLen,
              [in] ULONG cwcSrcPos);
    HRESULT PutAltWord(
              [in] ULONG cwc,
              [size_is(cwcSrcLen)][in] const WCHAR *pwcInBuf,
              [in] ULONG cwcSrcLen,
              [in] ULONG cwcSrcPos);
    HRESULT StartAltPhrase(void);
    HRESULT EndAltPhrase(void);
    HRESULT PutBreak(
              [in] WORDREP_BREAK_TYPE breakType);
}


/*****************************************************************************
 *    IPhraseSink interface
 */
[
    uuid(cc906ff0-c058-101a-b554-08002b33b0e6),
    object,
    local
]
interface IPhraseSink: IUnknown
{
    HRESULT PutSmallPhrase(
              [size_is(cwcNoun)][in] const WCHAR *pwcNoun,
              [in] ULONG cwcNoun,
              [size_is(cwcModifier)][in] const WCHAR *pwcModifier,
              [in] ULONG cwcModifier,
              [in] ULONG ulAttachmentType);
    HRESULT PutPhrase(
              [size_is(cwcPhrase)][in] const WCHAR *pwcPhrase,
              [in] ULONG cwcPhrase);
}


/*****************************************************************************
 *    IWordBreaker interface
 */
[
    uuid(D53552C8-77E3-101A-B552-08002B33B0E6),
    object,
    local
]
interface IWordBreaker: IUnknown
{
    HRESULT Init(
              [in] BOOL fQuery,
              [in] ULONG ulMaxTokenSize,
              [out] BOOL *pfLicense);
    HRESULT BreakText(
              [in] TEXT_SOURCE *pTextSource,
              [in] IWordSink *pWordSink,
              [in] IPhraseSink *pPhraseSink);
    HRESULT ComposePhrase(
              [size_is(cwcNoun)][in] const WCHAR *pwcNoun,
              [in] ULONG cwcNoun,
              [size_is(cwcModifier)][in] const WCHAR *pwcModifier,
              [in] ULONG cwcModifier,
              [in] ULONG ulAttachmentType,
              [size_is(*pcwcPhrase)][out] WCHAR *pwcPhrase,
              [out][in] ULONG *pcwcPhrase);
    HRESULT GetLicenseToUse(
              [string][out] const WCHAR **ppwcsLicense);
};

    
[
	uuid(fe77c330-7f42-11ce-be57-00aa0051fe20),
	object,
	local
]
interface IWordFormSink : IUnknown
{
    HRESULT PutAltWord( 
        [size_is(cwc)][in] const WCHAR *pwcInBuf,
        [in] ULONG cwc);
        
    HRESULT PutWord( 
        [size_is(cwc)][in] const WCHAR *pwcInBuf,
        [in] ULONG cwc);
        
};

[
	uuid(efbaf140-7f42-11ce-be57-00aa0051fe20),
	object,
	local
]
interface IStemmer : IUnknown
{
    HRESULT Init( 
        [in] ULONG ulMaxTokenSize,
        [out] BOOL *pfLicense);
        
    HRESULT GenerateWordForms( 
        [in] const WCHAR *pwcInBuf,
        [in] ULONG cwc,
        [in] IWordFormSink *pStemSink);
        
    HRESULT GetLicenseToUse( 
        [string][out] const WCHAR **ppwcsLicense);
        
};