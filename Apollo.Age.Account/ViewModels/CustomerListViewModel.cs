
﻿namespace Apollo.Age.Account.ViewModels
 {
     using System;

     public class CustomerListViewModel
     {
         public int CustId { get; set; }

         public string Username { get; set; }

         public Int32 Status { get; set; }

         public Int32 Handicap { get; set; }

         public Int32 OverUnder { get; set; }

         public Int32 HandicapLive { get; set; }

         public Int32 OverUnderLive { get; set; }

         public string UserGroup { get; set; }

         public decimal CustomerGroup { get; set; }

         public decimal CustomerDiscountOneXTwo { get; set; }

         public decimal CustomerDiscountOther { get; set; }

         public decimal CustDiscountNumberGame { get; set; }

         public decimal AgentGroup { get; set; }

         public decimal AgentDiscountOneXTwo { get; set; }

         public decimal AgentDiscountOther { get; set; }

         public decimal AgentDiscountNumberGame { get; set; }
     }

 }

