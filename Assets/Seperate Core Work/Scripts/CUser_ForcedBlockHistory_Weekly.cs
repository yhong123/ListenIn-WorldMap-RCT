using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//namespace Assets.Seperate_Core_Work.Scripts
//{
class CUser_ForcedBlockHistory_Weekly
{
    public string m_strDate = "";
    public List<CUser_ForcedBlockHistory_Daily> m_lsForcedBlockHistory_Daily = new List<CUser_ForcedBlockHistory_Daily>();

    //----------------------------------------------------------------------------------------------------
    // CUser_ForcedBlockHistory_Weekly
    //----------------------------------------------------------------------------------------------------
    public CUser_ForcedBlockHistory_Weekly()
    {
    }
}

class CUser_ForcedBlockHistory_Daily
{
    public string m_strDate = "";
    public List<int> m_lsBlockIdx = new List<int>();

    //----------------------------------------------------------------------------------------------------
    // CUser_ForcedBlockHistory_Daily
    //----------------------------------------------------------------------------------------------------
    public CUser_ForcedBlockHistory_Daily()
    {
    }
}
//}
