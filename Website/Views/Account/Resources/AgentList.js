/// <reference path="../../../_GlobalResources/Js/jquery.js" />

var AGENTLIST = function ( $ ) {
    return {
        AgentId: null,
        init: function () {
            AGENTLIST.AgentId = $( "#gridId" ).attr( "agentid" );
            AGENTLIST.loadAgentList();
        },
        loadNavigation: function () {
            $.ajax( {
                type: "POST",
                url: APOLLO.resolveUrl( "Account/GetAgentDownlinePath" ),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify( { AgentId: AGENTLIST.AgentId } ),
                timeout: 120000
            } )
                .done( function ( result ) {
                    var rowLength = result.length;
                    var template = '';

                    for ( var i = 0; i < rowLength; i++ ) {
                        if ( i == rowLength - 1 ) {
                            template += "<span>" + result[i].Username + "</span>";
                            $( "#currUsername" ).html( result[i].Username );
                            $( "#quickLinkEditSetting" ).attr( "agentid", result[i].Id );
                            $( "#quickLinkEditSetting" ).attr( "agentlevel", result[i].Level );
                        }
                        else {
                            template += "<a href='#' name='navigation' agentid='" + result[i].Id + "' >" + result[i].Username + "</a>";
                            template += "<img alt='' height='12' width='12' src= '" + APOLLO.resolveUrl( "../_MemberInfo/CustomerList/Resources/Images/breadcrumb.gif" ) + "'/>";
                        }
                    }
                    $( "#divnavigation" ).html( template );
                    AGENTLIST.bindingNavigation();
                } )
                .fail( function ( event, jqXHR, ajaxSettings ) {
                    APOLLO.handleAjaxError( event, jqXHR, ajaxSettings );
                } );
        },
        bindingViewAgent: function () {
            $( '[name="viewAgent"]' ).unbind( 'click' ).bind( 'click', function () {
                AGENTLIST.AgentId = $( this ).attr( 'agentid' );
                AGENTLIST.loadAgentList();
                return false;
            } );
        },
        bindingViewMember: function () {
            $( '[name="viewMember"]' ).unbind( 'click' ).bind( 'click', function () {
                AGENTLIST.AgentId = $( this ).attr( 'agentid' );

                location.href = APOLLO.resolveUrl( "Account/MemberList?agentId=" + AGENTLIST.AgentId + "&username=" + $( this ).attr( 'username' ) );
                return false;
            } );
        },
        bindingNavigation: function () {
            $( "[name='navigation']" ).unbind( 'click' ).bind( 'click', function () {
                AGENTLIST.AgentId = $( this ).attr( 'agentid' );
                AGENTLIST.loadAgentList();
                return false;
            } );

            $( ".edit-setting" ).mouseover( function () {
            } );
        },
        bindingQuickLink: function () {
            $( "#quickLinkCustomerList" ).unbind( 'click' ).bind( 'click', function () {
                location.href = APOLLO.resolveUrl( "Account/MemberList?agentid=" + AGENTLIST.AgentId );
                return false;
            } );
        },
        bindingSearch: function () {
            $( "#Search" ).unbind( 'click' ).bind( 'click', function () {
                AGENTLIST.loadAgentList( AGENTLIST.AgentId );
            } );
        },
        initMenuPopup: function ( els ) {
            var menuEdit = $( "#menuedit" );
            var commission = $( "#commission" );
            var betsetting = $( "#betsetting" );
            els.mouseover( function () {
                var pos = $( this ).offset();
                var eWidth = $( this ).outerWidth();
                var mWidth = menuEdit.outerWidth();
                var left = ( pos.left + eWidth ) + "px";
                var top = 3 + pos.top + "px";
                var agentid = $( this ).attr( 'agentid' );
                var agentlevel = $( this ).attr( 'agentlevel' );
                commission.attr( "agentId", agentid );
                betsetting.attr( "agentId", agentid );

                commission.attr( "agentlevel", agentlevel );
                betsetting.attr( "agentlevel", agentlevel );

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
                console.log( $( this ).attr( "agentid" ) );
                console.log( $( this ).attr( "agentlevel" ) );
                // Code here
            } );
            betsetting.unbind( 'click' ).bind( 'click', function () {
                console.log( $( this ).attr( "agentid" ) );
                console.log( $( this ).attr( "agentlevel" ) );
                // Code here
            } );
            menuEdit.mouseover( function () {
                $( this ).show();
            } ).mouseout( function () {
                $( this ).hide();
            } );
        },
        bindingEdit: function () {
            AGENTLIST.initMenuPopup( $( "[name='Edit']" ) );
            AGENTLIST.initMenuPopup( $( ".edit-setting" ) );
        },
        loadAgentList: function () {
            $.ajax( {
                type: "POST",
                url: APOLLO.resolveUrl( "Account/GetAgentList" ),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify( { AgentId: AGENTLIST.AgentId, Username: $( "#txtUserName" ).val() == BASELANGUAGES.EnterUsername ? "" : $( "#txtUserName" ).val() } ),
                timeout: 120000
            } )
                .done( function ( result ) {
                    AGENTLIST.AgentId = result.CurrentAgentId;
                    AGENTLIST.initAgentList( result );
                    AGENTLIST.loadNavigation();

                    AGENTLIST.bindingQuickLink();
                    AGENTLIST.bindingSearch();
                } )
                .fail( function ( event, jqXHR, ajaxSettings ) {
                    APOLLO.handleAjaxError( event, jqXHR, ajaxSettings );
                } );
        },

        initAgentList: function ( agentList ) {
            var bodyData = [];

            var rowLength = agentList.ListData.length;
            var counter = ( agentList.CurrentPage * agentList.ItemsPerPage );
            for ( var i = 0; i < rowLength; i++ ) {
                var current = agentList.ListData[i];
                counter++;
                bodyData.push( {
                    Columns:
                        [
                            { Name: "No", Value: counter },
                            { Name: "Edit", Value: current },
                            { Name: "Username", Value: current.Username },
                            { Name: "Status", Value: current.Status },
                            { Name: "NumberOfAgent", Value: current },
                            { Name: "NumberOfMember", Value: current },
                            { Name: "GroupA", Value: current.GroupA },
                            { Name: "GroupB", Value: current.GroupB },
                            { Name: "GroupC", Value: current.GroupC },
                            { Name: "GroupD", Value: current.GroupD },
                            { Name: "OneXTwo", Value: current.OneXTwo },
                            { Name: "Other", Value: current.Other },
                            { Name: "NumberGame", Value: current.NumberGame },
                            { Name: "Handicap", Value: current.Handicap },
                            { Name: "OverUnder", Value: current.OverUnder },
                            { Name: "HandicapLive", Value: current.HandicapLive },
                            { Name: "OverUnderLive", Value: current.OverUnderLive }
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
                                            { HeaderText: LANGUAGES.NumberOfAgent, Name: "NumberOfAgent", MergeRowNumber: 2 },
                                            { HeaderText: LANGUAGES.NumberOfMember, Name: "NumberOfMember", MergeRowNumber: 2 },
                                            { HeaderText: LANGUAGES.Commission, Name: "GroupA", MergeColNumber: 7 },
                                            { HeaderText: LANGUAGES.SoccerShare, Name: "Handicap", MergeColNumber: 4 }

                                        ]
                                 }
                        ],
                columns: [
                         { HeaderText: "#", Width: 30, Name: "No", HeaderAlign: "center", CellAlign: "center" },
                         { HeaderText: LANGUAGES.Edit, Width: 30, Name: "Edit", HeaderAlign: "center", CellAlign: "center",
                             CustomCellRender: function ( cell ) {
                                 return "<a href='#' class='edit-setting' name='Edit' agentid=" + cell.Value.AgentId + " agentlevel=" + cell.Value.AgentLevel + " ></a>";
                             }
                         },
                         { HeaderText: LANGUAGES.Username, Width: 150, Name: "Username", HeaderAlign: "center", CellAlign: "left" },
                         { HeaderText: LANGUAGES.Status, Width: 50, Name: "Status", HeaderAlign: "center", CellAlign: "center",
                             CustomCellRender: function ( cell ) {
                                 return ( cell.Value.Status == 1 ? LANGUAGES.Active : ( cell.Value.Status == 2 ? LANGUAGES.Stopped : LANGUAGES.Locked ) );
                             }
                         },
                         { HeaderText: LANGUAGES.NumberOfAgent, Width: 70, Name: "NumberOfAgent", HeaderAlign: "center", CellAlign: "center",
                             CustomCellRender: function ( cell ) {
                                 return ( cell.Value.NumberOfAgent != 0 ? "<a name='viewAgent' href='#' agentId=" + cell.Value.AgentId + ">" + cell.Value.NumberOfAgent + "</a>" : cell.Value.NumberOfAgent );
                             }
                         },
                         { HeaderText: LANGUAGES.NumberOfMember, Width: 70, Name: "NumberOfMember", HeaderAlign: "center", CellAlign: "center",
                             CustomCellRender: function ( cell ) {
                                 return ( cell.Value.NumberOfMember != 0 ? "<a href='#' name='viewMember' agentId=" + cell.Value.AgentId + " username=" + cell.Value.Username + ">" + cell.Value.NumberOfMember + "</a>" : cell.Value.NumberOfMember );
                             }
                         },
                         { HeaderText: LANGUAGES.GroupA, Width: 100, Name: "GroupA", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00" },
                         { HeaderText: LANGUAGES.GroupB, Width: 100, Name: "GroupB", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00" },
                         { HeaderText: LANGUAGES.GroupC, Width: 100, Name: "GroupC", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00" },
                         { HeaderText: LANGUAGES.GroupD, Width: 100, Name: "GroupD", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00" },
                         { HeaderText: LANGUAGES.OneXTwo, Width: 100, Name: "OneXTwo", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00" },
                         { HeaderText: LANGUAGES.Other, Width: 100, Name: "Other", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00" },
                         { HeaderText: LANGUAGES.NumberGame, Width: 100, Name: "NumberGame", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00" },
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
                    itemsPerPage: agentList.ItemsPerPage,
                    currentPage: agentList.CurrentPage,
                    totalItem: agentList.TotalItem,
                    itemsPerPageArray: [50, 100, 500],

                    pagerSelectCallBack: ReRenderAfterPaged
                },
                showColumnsShowHide: false,
                columnsNeedHide: [12]
            };
            function ReRenderAfterPaged( page_index, itemsPerPage, jq ) {
                $.ajax( {
                    type: "POST",
                    url: APOLLO.resolveUrl( "Account/GetAgentList" ),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: JSON.stringify( { agentId: AGENTLIST.AgentId, pageIndex: page_index, pageSize: itemsPerPage } ),
                    timeout: 120000
                } )
                    .done( function ( result ) {
                        gridOption.pagerOption.itemsPerPage = itemsPerPage;
                        gridOption.pagerOption.currentPage = page_index;
                        gridOption.pagerOption.totalItem = result.TotalItem;
                        AGENTLIST.AgentId = result.CurrentAgentId;
                        AGENTLIST.initAgentList( result );
                    } )
                    .fail( function ( event, jqXHR, ajaxSettings ) {
                        APOLLO.handleAjaxError( event, jqXHR, ajaxSettings );
                    } );
                return false;
            }
            $( "#gridId" ).fanexGrid( gridOption );

            AGENTLIST.bindingViewAgent();
            AGENTLIST.bindingViewMember();
            AGENTLIST.bindingEdit();
        }
    };
} ( jQuery );

$( document ).ready( function () {
    AGENTLIST.init();
} ).ajaxStart( $.blockUI ).ajaxStop( $.unblockUI );