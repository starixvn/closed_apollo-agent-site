/// <reference path="../../../_GlobalResources/Js/jquery.js" />

var MEMBERLIST = function ( $ ) {
    return {
        AgentId: null,
        currentPopUp: null,
        AgentLevel: null,
        init: function () {
            MEMBERLIST.AgentId = $( "#gridId" ).attr( "agentid" );
            MEMBERLIST.AgentLevel = $( "#gridId" ).attr( "agentlevel" );
            $( "#currUsername" ).html( $( "#gridId" ).attr( "username" ) );
            MEMBERLIST.loadMemberList();
            MEMBERLIST.bindingNavigation();
            MEMBERLIST.bindingQuickiLinkEditSetting( MEMBERLIST.AgentId, MEMBERLIST.AgentLevel );
        },
        bindingQuickiLinkEditSetting: function ( agentId, agentLevel ) {
            var $quickLinkEditSetting = $( "#quickLinkEditSetting" );
            $quickLinkEditSetting.attr( "agentid", agentId );
            $quickLinkEditSetting.attr( "agentlevel", agentLevel );
        },
        bindingNavigation: function () {
            $( "[name='navigation']" ).unbind( 'click' ).bind( 'click', function () {
                location.href = APOLLO.resolveUrl( "Account/Agentlist?agentid=" + $( this ).attr( 'agentid' ) );
                return false;
            } );
        },
        bindingQuickLink: function () {
            $( "#quickLinkAgentList" ).unbind( 'click' ).bind( 'click', function () {
                location.href = APOLLO.resolveUrl( "Account/AgentList?agentid=" + MEMBERLIST.AgentId );
            } );
        },
        bindingSearch: function () {
            $( "#Search" ).unbind( 'click' ).bind( 'click', function () {
                MEMBERLIST.loadMemberList( MEMBERLIST.AgentId );
            } );
        },
        editCustomerSetting: function ( $this ) {
            var url = APOLLO.resolveUrl( "CustomerSetting/EditAllSetting?custId=" + $this.attr( "custid" )
                                + "&userName=" + $this.attr( "username" )
                                + "&userGroup=" + $this.attr( "usergroup" )
                                + "&agentId=" + $( "#gridId" ).attr( "agentid" ) );
            MEMBERLIST.currentPopUp = APOLLO.openDialogFrame( url, 'Edit Member Setting - ' + $this.attr( "username" ), {
                width: 1015,
                height: 450,
                position: { my: "top+25%", at: "center top", of: window },
                close: function ( event, ui ) {
                }
            } );
        },
        bindingEdit: function () {
            $( "[name='Edit']" ).unbind( 'click' ).bind( 'click', function () {
                MEMBERLIST.editCustomerSetting( $( this ) );
            } );
            $( "#quickLinkEditSetting" ).unbind( 'click' ).bind( 'click', function () {
                // Edit Agent
                var $this = $( this );
                var url = APOLLO.resolveUrl( "CustomerSetting/EditAllSetting?custId=" + $this.attr( "agentid" )
                + "&agentLevel=" + $this.attr( "agentlevel" ) );
                MEMBERLIST.currentPopUp = APOLLO.openDialogFrame( url, 'Edit Agent Setting ', {
                    width: 1015,
                    height: 450,
                    position: { my: "top+25%", at: "center top", of: window },
                    close: function ( event, ui ) {
                    }
                } );
            } );
        },
        loadMemberList: function () {
            $.ajax( {
                type: "POST",
                url: APOLLO.resolveUrl( "Account/GetMemberList" ),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify( { AgentId: MEMBERLIST.AgentId, Username: $( "#txtUserName" ).val() == BASELANGUAGES.EnterUsername ? "" : $( "#txtUserName" ).val() } ),
                timeout: 120000
            } )
                .done( function ( result ) {
                    MEMBERLIST.AgentId = result.CurrentAgentId;
                    MEMBERLIST.initMemberList( result );

                    MEMBERLIST.bindingQuickLink();
                    MEMBERLIST.bindingSearch();
                } )
                .fail( function ( event, jqXHR, ajaxSettings ) {
                    APOLLO.handleAjaxError( event, jqXHR, ajaxSettings );
                } );
            if ( MEMBERLIST.currentPopUp != null ) {
                APOLLO.closeDialog( MEMBERLIST.currentPopUp );
            }
        },
        initMemberList: function ( memberList ) {
            var bodyData = [];

            var rowLength = memberList.ListData.length;
            var counter = ( memberList.CurrentPage * memberList.ItemsPerPage );
            for ( var i = 0; i < rowLength; i++ ) {
                var current = memberList.ListData[i];
                counter++;
                bodyData.push( {
                    Columns:
                        [
                            { Name: "No", Value: counter },
                            { Name: "Edit", Value: current },
                            { Name: "Username", Value: current.Username },
                            { Name: "Status", Value: current.Status },
                            { Name: "BetStatus", Value: current.BetStatus },
                            { Name: "UserGroup", Value: current.UserGroup },
                            { Name: "AgentGroup", Value: current.AgentGroup * 100 },
                            { Name: "AgentOneXTwo", Value: current.AgentDiscountOneXTwo * 100 },
                            { Name: "AgentOther", Value: current.AgentDiscountOther * 100 },
                            { Name: "CustomerGroup", Value: current.CustomerGroup * 100 },
                            { Name: "CustomerOneXTwo", Value: current.CustomerDiscountOneXTwo * 100 },
                            { Name: "CustomerOther", Value: current.CustomerDiscountOther * 100 },
                            { Name: "HandicapTakenShare", Value: current.HandicapTakenShare / 100 },
                        ]
                } );
            }

            var gridOption = {
                customizeHeader: true,
                groupHeaderRows: [
                                 {
                                     Columns: [
                                            { HeaderText: "#", Name: "No", MergeRowNumber: 2 },
                                            { HeaderText: LANGUAGES.Edit, Name: "Edit", MergeRowNumber: 2 },
                                            { HeaderText: LANGUAGES.Username, Name: "Username", MergeRowNumber: 2 },
                                            { HeaderText: LANGUAGES.Status, Name: "Status", MergeRowNumber: 2 },
                                            { HeaderText: LANGUAGES.BetStatus, Name: "BetStatus", MergeRowNumber: 2 },
                                            { HeaderText: LANGUAGES.Group, Name: "UserGroup", MergeRowNumber: 2 },
                                            { HeaderText: LANGUAGES.AgentCommission, Name: "AgentGroup", MergeColNumber: 3 },
                                            { HeaderText: LANGUAGES.MemberCommission, Name: "CustomerGroup", MergeColNumber: 3 },
                                            { HeaderText: LANGUAGES.SoccerDeadballHDPTakenShare, Name: "HandicapTakenShare", MergeRowNumber: 2 },

                                        ]
                                 }
                        ],
                columns: [
                         { HeaderText: "#", Width: 30, Name: "No", HeaderAlign: "center", CellAlign: "center" },
                         { HeaderText: LANGUAGES.Edit, Width: 30, Name: "Edit", HeaderAlign: "center", CellAlign: "center",
                             CustomCellRender: function ( cell ) {
                                 return "<a href='#' class='edit-setting icon-pencil' name='Edit' usergroup=" + cell.Value.UserGroup + " username=" + cell.Value.Username + " custid=" + cell.Value.CustId + " ></a>";
                             }
                         },
                         { HeaderText: LANGUAGES.Username, Width: 150, Name: "Username", HeaderAlign: "center", CellAlign: "left" },
                         { HeaderText: LANGUAGES.Status, Width: 50, Name: "Status", HeaderAlign: "center", CellAlign: "center",
                             CustomCellRender: function ( cell ) {
                                 return ( cell.Value == 1 ? "<span class='active'>" + LANGUAGES.Active + "</span>" : ( cell.Value == 2 ? "<span class='stop'>" + LANGUAGES.Stopped + "</span>" : "<span class='lock'>" + LANGUAGES.Locked + "</span>" ) );
                             }
                         },
                         { HeaderText: LANGUAGES.BetStatus, Width: 100, Name: "BetStatus", HeaderAlign: "center", CellAlign: "center",
                             CustomCellRender: function ( cell ) {
                                 return ( cell.Value == 1 ? "<span class='active'>" + LANGUAGES.Active + "</span>" : ( cell.Value == 2 ? "<span class='stop'>" + LANGUAGES.Stopped + "</span>" : "<span class='lock'>" + LANGUAGES.Locked + "</span>" ) );
                             }
                         },
                         { HeaderText: LANGUAGES.Group, Width: 100, Name: "UserGroup", HeaderAlign: "center", CellAlign: "center" },

                         { HeaderText: LANGUAGES.Group, Width: 100, Name: "AgentGroup", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00" },
                         { HeaderText: LANGUAGES.OneXTwo, Width: 100, Name: "AgentOneXTwo", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00" },
                         { HeaderText: LANGUAGES.Other, Width: 100, Name: "AgentOther", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00" },

                         { HeaderText: LANGUAGES.Group, Width: 100, Name: "CustomerGroup", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00" },
                         { HeaderText: LANGUAGES.OneXTwo, Width: 100, Name: "CustomerOneXTwo", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00" },
                         { HeaderText: LANGUAGES.Other, Width: 100, Name: "CustomerOther", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00" },

                         { HeaderText: LANGUAGES.SoccerDeadballHDPTakenShare, Width: 150, Name: "HandicapTakenShare", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00" },

                        ],
                bodyRows: bodyData,
                showFooter: false,
                fullHeight: true,
                gridExpandHeight: 250,
                numberOfFrozenColumn: 3,
                marginRight: 0,
                showPager: true,
                customPager: true,
                pagerOption: {
                    itemsPerPage: memberList.ItemsPerPage,
                    currentPage: memberList.CurrentPage,
                    totalItem: memberList.TotalItem,
                    itemsPerPageArray: [10, 20, 50, 100, 200, 500],
                    pagerSelectCallBack: ReRenderAfterPaged
                }
            };
            function ReRenderAfterPaged( page_index, itemsPerPage, jq ) {
                $.ajax( {
                    type: "POST",
                    url: APOLLO.resolveUrl( "Account/GetMemberList" ),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: JSON.stringify( { agentId: MEMBERLIST.AgentId, pageIndex: page_index, pageSize: itemsPerPage } ),
                    timeout: 120000
                } )
                    .done( function ( result ) {
                        gridOption.pagerOption.itemsPerPage = itemsPerPage;
                        gridOption.pagerOption.currentPage = page_index;
                        gridOption.pagerOption.totalItem = result.TotalItem;
                        MEMBERLIST.AgentId = result.CurrentAgentId;

                        MEMBERLIST.initMemberList( result );
                    } )
                    .fail( function ( event, jqXHR, ajaxSettings ) {
                        APOLLO.handleAjaxError( event, jqXHR, ajaxSettings );
                    } );
                return false;
            }
            $( "#gridId" ).fanexGrid( gridOption );

            MEMBERLIST.bindingEdit();
        }
    };
} ( jQuery );

$( document ).ready( function () {
    MEMBERLIST.init();
} ).ajaxStart( $.blockUI ).ajaxStop( $.unblockUI );