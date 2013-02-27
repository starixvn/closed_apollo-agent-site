namespace Sunplus.Agent.UI.BetList.Factories
{
    using System;
    using BetTypes;

    public class BetItemFactory
    {
        #region Bet Item factory

        /// <summary>
        /// Create IBetItem instance by reflect on bet type id.
        /// </summary>
        /// <param name="betType"></param>
        /// <returns></returns>
        internal static IBetItem CreateBetItem(int betType)
        {
            string betTypeId = betType.ToString();
            string typeName = string.Join(string.Empty, new string[] { "Sunplus.Agent.UI.BetList.BetTypes", ".BetType", betTypeId });

            IBetItem betItem = null;
            Type type = Type.GetType(typeName);
            if (type == null)
            {
                betItem = new BetType1();
            }
            else
            {
                betItem = Activator.CreateInstance(type) as IBetItem;
            }

            // Other initialize here
            return betItem;
        }

        /// <summary>
        /// Create default bet item.
        /// </summary>
        /// <returns></returns>
        internal static IBetItem CreateBetItem()
        {
            return CreateBetItem(1);
        }

        /// <summary>
        /// Create bet list header base on bet list type.
        /// </summary>
        /// <param name="betListType"></param>
        /// <returns></returns>
        internal static IBetItem CreateHeader(BetListType betListType)
        {
            string betListTypeId = ((int)betListType).ToString();
            string typeName = string.Empty;

            // Choose template use for SmartPhone
            if ((betListType & BetListType.SmartPhone) == BetListType.SmartPhone)
            {
                typeName = string.Join(string.Empty, new string[] { "Sunplus.Agent.UI.BetList.HeaderSmartPhone", ".HeaderSmartPhone" });
            }
            else
            {
                typeName = string.Join(string.Empty, new string[] { "Sunplus.Agent.UI.BetList.Header", ".Header", betListTypeId });
            }

            IBetItem header = null;
            Type type = Type.GetType(typeName);
            if (type == null)
            {
                header = new Header.Header1();
            }
            else
            {
                header = Activator.CreateInstance(type) as IBetItem;
            }

            return header;
        }

        /// <summary>
        /// Create default bet list header.
        /// </summary>
        /// <returns></returns>
        internal static IBetItem CreateHeader()
        {
            return CreateHeader(BetListType.Default);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="footerType"></param>
        /// <returns></returns>
        internal static IBetItem CreateFooter(FooterType footerType, BetListType betListType)
        {      
            string typeName = string.Empty;

            // Choose template use for SmartPhone
            if (((betListType & BetListType.SmartPhone) == BetListType.SmartPhone) && footerType != FooterType.None)
            {
                typeName = string.Join(string.Empty, new string[] { "Sunplus.Agent.UI.BetList.FooterSmartPhone", ".FooterSmartPhone" });
            }
            else
            {
                typeName = string.Join(string.Empty, new string[] { "Sunplus.Agent.UI.BetList.Footer", ".Footer", ((int)footerType).ToString() });
            }

            IBetItem footer = null;
            Type type = Type.GetType(typeName);
            if (type == null)
            {
                footer = new Footer.Footer1();
            }
            else
            {
                footer = Activator.CreateInstance(type) as IBetItem;
            }

            return footer;
        }

        /// <summary>
        /// Create default bet list footer.
        /// </summary>
        /// <returns></returns>
        internal static IBetItem CreateFooter()
        {
            return CreateFooter(FooterType.Total, BetListType.Default);
        }
        #endregion
    }
}
