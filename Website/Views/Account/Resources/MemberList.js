/// <reference path="../../../_GlobalResources/Js/jquery.js" />

var MEMBERLIST = function ( $ ) {
    return {
        AgentId: null,
        init: function () {
            MEMBERLIST.AgentId = $( "#gridId" ).attr( "agentid" );
            $( "#currUsername" ).html( $( "#gridId" ).attr( "username" ) );
            MEMBERLIST.loadMemberList();
            MEMBERLIST.bindingNavigation();
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
        initMenuPopup: function ( els ) {
            var menuEdit = $( "#menuedit" );
            var commission = $( "#commission" );
            var betsetting = $( "#betsetting" );
            var sharesetting = $( "#sharesetting" );
            els.unbind( 'click' ).bind( 'click', function () {
                //code here
            } );
            els.mouseover( function () {
                var pos = $( this ).offset();
                var eWidth = $( this ).outerWidth();
                var mWidth = menuEdit.outerWidth();
                var left = ( pos.left + eWidth ) + "px";
                var top = 3 + pos.top + "px";
                commission.attr( "custid", $( this ).attr( 'custid' ) );
                commission.attr( "username", $( this ).attr( 'username' ) );
                commission.attr( "usergroup", $( this ).attr( 'usergroup' ) );

                betsetting.attr( "custid", $( this ).attr( 'custid' ) );
                betsetting.attr( "username", $( this ).attr( 'username' ) );
                betsetting.attr( "usergroup", $( this ).attr( 'usergroup' ) );

                sharesetting.attr( "custid", $( this ).attr( 'custid' ) );
                sharesetting.attr( "username", $( this ).attr( 'username' ) );
                sharesetting.attr( "usergroup", $( this ).attr( 'usergroup' ) );

                menuEdit.css( {
                    position: 'absolute',
                    zIndex: 5000,
                    left: left,
                    top: top
                } );
                menuEdit.show();
            } ).mouseout( function () {
                menuEdit.hide();
            } );
            commission.unbind( 'click' ).bind( 'click', function () {
                var $this = $( this );
                var url = APOLLO.resolveUrl( "CustomerSetting/EditCommissionSetting?custId=" + $this.attr( "custid" )
                + "&userName" + $this.attr( "username" )
                + "&userGroup=" + $this.attr( "usergroup" ) );
                APOLLO.openDialogFrame( url, '', {
                    width: 1015,
                    height: 300,
                    position: { my: "top+50", at: "center top", of: window },
                    close: function ( event, ui ) {
                    }
                } );

                // Code here
            } );
            betsetting.unbind( 'click' ).bind( 'click', function () {
                var $this = $( this );
                var url = APOLLO.resolveUrl( "CustomerSetting/EditBetSetting?custId=" + $this.attr( "custid" )
                + "&userName" + $this.attr( "username" ) );
                APOLLO.openDialogFrame( url, '', {
                    width: 1015,
                    height: 600,
                    position: { my: "top+50", at: "center top", of: window },
                    close: function ( event, ui ) {
                    }
                } );
            } );
            sharesetting.unbind('click').bind('click', function () {
                var $this = $(this);
                var url = APOLLO.resolveUrl("CustomerSetting/EditPositionTaking?custId=" + $this.attr("custid")
                + "&userName=" + $this.attr("username"));
                APOLLO.openDialogFrame(url, '', {
                    width: 1020,
                    height: 800,
                    position: { my: "top+10", at: "center top", of: window },
                    close: function (event, ui) {
                    }
                });
            } );

            menuEdit.mouseover( function () {
                $( this ).show();
            } ).mouseout( function () {
                $( this ).hide();
            } );
        },
        bindingEdit: function () {
            MEMBERLIST.initMenuPopup( $( "[name='Edit']" ) );
            MEMBERLIST.initMenuPopup( $( ".edit-setting" ) );
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
                            { Name: "Status", Value: current },
                            { Name: "UserGroup", Value: current.UserGroup },
                            { Name: "AgentGroup", Value: current.AgentGroup * 100 },
                            { Name: "AgentOneXTwo", Value: current.AgentDiscountOneXTwo * 100 },
                            { Name: "AgentOther", Value: current.AgentDiscountOther * 100 },
                            { Name: "AgentNumberGame", Value: current.AgentDiscountNumberGame * 100 },
                            { Name: "CustomerGroup", Value: current.CustomerGroup * 100 },
                            { Name: "CustomerOneXTwo", Value: current.CustomerDiscountOneXTwo * 100 },
                            { Name: "CustomerOther", Value: current.CustomerDiscountOther * 100 },
                            { Name: "CustomerNumberGame", Value: current.CustDiscountNumberGame * 100 },
                            { Name: "Handicap", Value: current.Handicap / 100 },
                            { Name: "OverUnder", Value: current.OverUnder / 100 },
                            { Name: "HandicapLive", Value: current.HandicapLive / 100 },
                            { Name: "OverUnderLive", Value: current.OverUnderLive / 100 }
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
                                            { HeaderText: LANGUAGES.Group, Name: "UserGroup", MergeRowNumber: 2 },

                                            { HeaderText: LANGUAGES.AgentCommission, Name: "AgentGroup", MergeColNumber: 4 },
                                            { HeaderText: LANGUAGES.MemberCommission, Name: "CustomerGroup", MergeColNumber: 4 },
                                            { HeaderText: LANGUAGES.SoccerShare, Name: "Handicap", MergeColNumber: 4 }

                                        ]
                                 }
                        ],
                columns: [
                         { HeaderText: "#", Width: 30, Name: "No", HeaderAlign: "center", CellAlign: "center" },
                         { HeaderText: LANGUAGES.Edit, Width: 30, Name: "Edit", HeaderAlign: "center", CellAlign: "center",
                             CustomCellRender: function ( cell ) {
                                 return "<a href='#' class='edit-setting' name='Edit' usergroup=" + cell.Value.UserGroup + " username=" + cell.Value.Username + " custid=" + cell.Value.CustId + " ></a>";
                             }
                         },
                         { HeaderText: LANGUAGES.Username, Width: 150, Name: "Username", HeaderAlign: "center", CellAlign: "left" },
                         { HeaderText: LANGUAGES.Status, Width: 50, Name: "Status", HeaderAlign: "center", CellAlign: "center",
                             CustomCellRender: function ( cell ) {
                                 return ( cell.Value.Status == 1 ? LANGUAGES.Active : ( cell.Value.Status == 2 ? LANGUAGES.Stopped : LANGUAGES.Locked ) );
                             }
                         },
                         { HeaderText: LANGUAGES.Group, Width: 100, Name: "UserGroup", HeaderAlign: "center", CellAlign: "center" },

                         { HeaderText: LANGUAGES.Group, Width: 100, Name: "AgentGroup", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00" },
                         { HeaderText: LANGUAGES.OneXTwo, Width: 100, Name: "AgentOneXTwo", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00" },
                         { HeaderText: LANGUAGES.Other, Width: 100, Name: "AgentOther", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00" },
                         { HeaderText: LANGUAGES.NumberGame, Width: 100, Name: "AgentNumberGame", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00" },

                         { HeaderText: LANGUAGES.Group, Width: 100, Name: "CustomerGroup", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00" },
                         { HeaderText: LANGUAGES.OneXTwo, Width: 100, Name: "CustomerOneXTwo", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00" },
                         { HeaderText: LANGUAGES.Other, Width: 100, Name: "CustomerOther", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00" },
                         { HeaderText: LANGUAGES.NumberGame, Width: 100, Name: "CustomerNumberGame", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00" },

                         { HeaderText: LANGUAGES.Handicap, Width: 100, Name: "Handicap", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00" },
                         { HeaderText: LANGUAGES.OverUnder, Width: 100, Name: "OverUnder", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00" },
                         { HeaderText: LANGUAGES.HandicapLive, Width: 100, Name: "HandicapLive", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00" },
                         { HeaderText: LANGUAGES.OverUnderLive, Width: 100, Name: "OverUnderLive", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00" }

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
                    itemsPerPageArray: [50, 100, 500],
                    pagerSelectCallBack: ReRenderAfterPaged
                },
                showColumnsShowHide: false,
                columnsNeedHide: [8, 12]
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