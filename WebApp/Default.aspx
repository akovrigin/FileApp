<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="WebApp._Default" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    
    <telerik:RadScriptBlock runat="server" ID="RadScriptBlock1">
        <script type="text/javascript">
            //<![CDATA[
            Sys.Application.add_load(function () {
                demo.treeView = $find("<%= RadTreeView1.ClientID %>");
            });
            //]]>
        </script>
    </telerik:RadScriptBlock>

    <asp:HiddenField ID="MainFolder" ClientIDMode="Static" runat="server" />

    <div class="jumbotron">
        <h2><a href="FileApp.doc">ASP.NET File Application</a></h2>
        <p class="lead">File Application is an ASP.NET AJAX application that lets the user upload, store, browse, and copy folders and files on a web server.</p>
        <p></p>
    </div>
    
    <div class="row">
        <div class="col-md-4">
            <h4>Folders and files</h4>
            <p>
                <telerik:RadTreeView ID="RadTreeView1" 
                    RenderMode="Lightweight"  Width="100%" Height="40em"
                    OnClientNodeClicking="onNodeClicking"
                    OnClientNodeExpanded="onNodeExpanded"
                    OnClientContextMenuItemClicked="onContextMenuItemClicked"
                    runat="server">
                    <Nodes>
                        <telerik:RadTreeNode Text="Root" Expanded="False">
                            <Nodes>
                                <telerik:RadTreeNode Text="FakeNodes">
                                </telerik:RadTreeNode>
                            </Nodes>
                        </telerik:RadTreeNode>
                    </Nodes>
                    <ContextMenus>
                        <telerik:RadTreeViewContextMenu ID="ContextMenuFolder" runat="server">
                            <Items>
                                <telerik:RadMenuItem runat="server" PostBack="False" Text="Delete" Value="1">
                                </telerik:RadMenuItem>
                                <telerik:RadMenuItem runat="server" PostBack="False" Text="Rename" Value="2">
                                </telerik:RadMenuItem>
                                <telerik:RadMenuItem runat="server" PostBack="False" Text="Copy" Value="3">
                                </telerik:RadMenuItem>
                                <telerik:RadMenuItem runat="server" PostBack="False" Text="Upload" Value="4">
                                </telerik:RadMenuItem>
                            </Items>
                        </telerik:RadTreeViewContextMenu>
                        <telerik:RadTreeViewContextMenu ID="ContextMenuFile" runat="server">
                            <Items>
                                <telerik:RadMenuItem runat="server" PostBack="False" Text="Delete" Value="1">
                                </telerik:RadMenuItem>
                                <telerik:RadMenuItem runat="server" PostBack="False" Text="Rename" Value="2">
                                </telerik:RadMenuItem>
                                <telerik:RadMenuItem runat="server" PostBack="False" Text="Copy" Value="3">
                                </telerik:RadMenuItem>
                                <telerik:RadMenuItem runat="server" PostBack="False" Text="Download" Value="5">
                                </telerik:RadMenuItem>
                            </Items>
                        </telerik:RadTreeViewContextMenu>
                        <telerik:RadTreeViewContextMenu ID="ContextMenuRoot" runat="server">
                            <Items>
                                <telerik:RadMenuItem runat="server" PostBack="False" Text="Just select something else" Value="100">
                                </telerik:RadMenuItem>
                            </Items>
                        </telerik:RadTreeViewContextMenu>
                    </ContextMenus>
                </telerik:RadTreeView>
            </p>
            <p>
                <a class="btn btn-default" href="https://go.microsoft.com/fwlink/?LinkId=301948">Task description &raquo;</a>
                
                <a class="btn btn-primary" onclick="getChildren()">Тестирование ajax</a>
            </p>
        </div>
        <div class="col-md-8">
            <h2>Info</h2>
            <div>
                <div>Meta-data: </div>
                <asp:Label ID="meta" ClientIDMode="Static" runat="server" Text="0"></asp:Label>
            </div>
            <p>
                Browse file
            </p>
            <p>
                <a class="btn btn-default" href="https://go.microsoft.com/fwlink/?LinkId=301949">Learn more &raquo;</a>
            </p>
        </div>
    </div>

    <script>
        var demo = window.demo = window.demo || {};

        var fileImg = '<%=Microsoft.AspNet.FriendlyUrls.FriendlyUrl.Resolve("/images/file.ico")%>';
        var folderImg = '<%=Microsoft.AspNet.FriendlyUrls.FriendlyUrl.Resolve("/images/folder.ico")%>';

        document.addEventListener("DOMContentLoaded",
            function () {
                var treeView = demo.treeView;
                treeView.trackChanges();

                var node0 = treeView.get_nodes().getNode(0);
                node0.set_imageUrl(folderImg);
                node0.set_text(document.getElementById('MainFolder').value);
                node0.set_contextMenuID('ContextMenuRoot');
            });

        window.addNode = function (currentNode, id, text, isFolder) {

            var treeView = demo.treeView;
            treeView.trackChanges();

            //Instantiate a new client node
            var node = new Telerik.Web.UI.RadTreeNode();
            //Set its text
            node.set_text(text);

            var img = isFolder ? folderImg : fileImg;

            node.set_contextMenuID(isFolder ? 'ContextMenuFolder' : 'ContextMenuFile');

            node.set_imageUrl(img);
            //Add the new node as the child of the selected node or the treeview if no node is selected
            var parent = treeView.get_selectedNode() || treeView;

            if (currentNode == null) {
                //alert('1 - ' + text);
                parent.get_nodes().add(node);
                //alert('2 - ' + text);
            } else {
                //alert('3 - ' + text);
                currentNode.get_nodes().add(node);
                //alert('4 - ' + text);
            }

            //Expand the parent if it is not the treeview
            if (parent != treeView && !parent.get_expanded())
                parent.set_expanded(true);

            node.set_toolTip(parent.get_toolTip() + '\\' + text);

            if (isFolder) {
                addNode(node, 0, 'fake', false);
            }

            treeView.commitChanges();
            return false;
        };

        function onContextMenuItemClicked(sender, args) {

            var operation = args.get_menuItem().get_value();

            if (operation == 100)
                return;

            var node = args.get_node();

            node.select();

            var newName = null;

            if (operation == 2) {
                newName = prompt("Please enter new name", node.get_text()).trim();
                if (newName == '')
                    return;
                node.set_text(newName);
            }

            request(operation, node, isFolder(node), node.get_toolTip(), newName);

            if (operation == 1) {
                node.get_parent().get_nodes().remove(node);
            }
            else if (operation == 2) {
                node.get_nodes().clear();
                node.set_toolTip(node.get_parent().get_toolTip() + '\\' + newName);
            }
        }

        function onNodeClicking(sender, args) {
            //alert("OnClientNodeClicking: " + args.get_node().get_text());
//            var path = '';
//            path = args.get_node().get_toolTip();
//            getMetaData(args.get_node(), path);

            onNodeExpanded(sender, args);
        }

        function onNodeExpanded(sender, args) {
            //alert("OnClientNodeExpanded: " + args.get_node().get_text());
            args.get_node().select();

            var treeView = demo.treeView;
            var parent = treeView.get_selectedNode();
            parent.get_nodes().clear();

            request(0, args.get_node(), isFolder(args.get_node()), args.get_node().get_toolTip(), null);
        }

        function isFolder(node) {
            return node.get_imageUrl() === folderImg;
        }

        function setChildren(node, elements) {
            //node.get_nodes().clear();
            document.getElementById('meta').innerText = elements.Meta;
            for (var i = 0; i < elements.Items.length; i++) {
                var el = elements.Items[i];
                window.addNode(null, el.Id, el.Name, el.IsFolder);
            }
        }

        function request(operation, node, isFolder, path, option) {

            path = path.replace(/\\/g, '|');

            $.ajax({
                type: "POST",
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                url: "Default.aspx/GetChildren",
                data: "{'operation' :'" + operation + "', 'isFolder': '" + isFolder + "', 'path':'" + path + "', 'option' : '" + option + "'}",
                success: function(success) {
                    //alert("Success: " + success.d);
                    if (isFolder || operation != 2)
                        setChildren(node, JSON.parse(success.d));
                },
                error: function(xhr, textStatus, error) {
                    alert("Error: " + error);
                }
            });
        }

        function getMetaData(currentNode, path) {

            path = path.replace(/\\/g, '|');

            $.ajax({
                type: "POST",
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                url: "Default.aspx/GetMetaData",
                data: "{'path':'" + path + "'}",
                success: function (success) {
                    //alert("Success: " + success.d);
                    document.getElementById('meta').innerText = success.d;
                },
                error: function (xhr, textStatus, error) {
                    alert("Error: " + error);
                }
            });
        }

    </script>

</asp:Content>

