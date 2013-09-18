// dllmain.h : Declaration of module class.

class CTwitterFullTextLibModule : public ATL::CAtlDllModuleT< CTwitterFullTextLibModule >
{
public :
	DECLARE_LIBID(LIBID_TwitterFullTextLib)
	DECLARE_REGISTRY_APPID_RESOURCEID(IDR_TWITTERFULLTEXTLIB, "{458AA8B7-2B7D-4C78-8FDA-179FBF36A13E}")
};

extern class CTwitterFullTextLibModule _AtlModule;
