using System;
using System.Collections.Generic;
using System.Web;

namespace Sunplus.Agent.UI.BetList.Footer
{
    using BetTypes;
    using Sunplus.Agent.Model.BL.Definitions;
    using Sunplus.Agent.UI.BetList.Excel;

    /// <summary>
    /// Empty footer
    /// </summary>
    public class Footer2 : Footer2_Template, IBetItem
    {
        int totalColumns = 11; // Have maximum 11 columns

        #region IBetItem Members

        public string Render()
        {
            throw new NotImplementedException();
        }

        public void Assign(int index, System.Data.DataRow row, BetListWorkbook workbook)
        {
        }

        public void Assign(int index, System.Data.DataRow row)
        {
         
        }

        public void CustomizeColumns(UserTypes userType, BetListType betListType, BetListWorkbook workbook)
        {
        }

        public void CustomizeColumns(UserTypes userType, BetListType betListType)
        {
            //Hide Status: When view on SmartPhone
            if ((betListType & BetListType.SmartPhone) == BetListType.SmartPhone)
            {
                totalColumns = 9;

                if ((betListType & BetListType.Canceled) == BetListType.Canceled)
                {
                    totalColumns = totalColumns - 4;
                }
                else
                {

                    if ((betListType & BetListType.Member) != BetListType.Member)
                    {
                        totalColumns = totalColumns - 1;
                    }

                    if ((betListType & BetListType.Winlost) != BetListType.Winlost)
                    {
                        totalColumns = totalColumns - 1;
                    }


                    switch (userType)
                    {
                        case UserTypes.Super://super 
                            totalColumns = totalColumns - 2;
                            break;
                        case UserTypes.Master://master
                            totalColumns = totalColumns - 2;
                            break;
                        case UserTypes.Agent://agent
                            totalColumns = totalColumns - 2;
                            break;
                        default: // Hide all position taking
                            totalColumns = totalColumns - 3;
                            break;

                    }
                }
            }
            else
            {
                if ((betListType & BetListType.Canceled) == BetListType.Canceled)
                {
                    totalColumns = totalColumns - 4;
                }
                else
                {

                    if ((betListType & BetListType.Member) != BetListType.Member)
                    {
                        totalColumns = totalColumns - 1;
                    }

                    if ((betListType & BetListType.Winlost) != BetListType.Winlost)
                    {
                        totalColumns = totalColumns - 1;
                    }


                    switch (userType)
                    {
                        case UserTypes.Super://super 
                            break;
                        case UserTypes.Master://master
                            totalColumns = totalColumns - 1;
                            break;
                        case UserTypes.Agent://agent
                            totalColumns = totalColumns - 2;
                            break;
                        default: // Hide all position taking
                            totalColumns = totalColumns - 3;
                            break;

                    }
                }
            }
            this.colSpan = totalColumns.ToString();
        }

        #endregion

        #region IComponent Members

        public void HideLinkedResources()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IBetItem Members

        public List<int> ScoreMapBetTypes { get; set; }

        public bool IsAdminCanViewVNIP { get; set; }

        public bool ExportExcel { get; set; }

        public string Culture { get; set; }

        public System.Data.DataTable OutDataTable { get; set; }

        #endregion
    }
}
