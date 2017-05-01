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
        <div class="col-md-4 col-sm-4">
            <h3>Folders and files</h3>
            <p>
                <telerik:RadTreeView ID="RadTreeView1" 
                    RenderMode="Lightweight"  Width="100%" Height="100%"
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
                                <telerik:RadMenuItem runat="server" PostBack="False" Text="New" Value="4">
                                </telerik:RadMenuItem>
                                <telerik:RadMenuItem runat="server" PostBack="False" Text="Upload" Value="5">
                                </telerik:RadMenuItem>
                                <telerik:RadMenuItem runat="server" PostBack="False" Text="Copy" Value="3">
                                </telerik:RadMenuItem>
                                <telerik:RadMenuItem runat="server" PostBack="False" Text="Rename" Value="2">
                                </telerik:RadMenuItem>
                                <telerik:RadMenuItem runat="server" PostBack="False" Text="Delete" Value="1">
                                </telerik:RadMenuItem>
                            </Items>
                        </telerik:RadTreeViewContextMenu>
                        <telerik:RadTreeViewContextMenu ID="ContextMenuFile" runat="server">
                            <Items>
                                <telerik:RadMenuItem runat="server" PostBack="False" Text="Download" Value="6">
                                </telerik:RadMenuItem>
                                <telerik:RadMenuItem runat="server" PostBack="False" Text="Copy" Value="3">
                                </telerik:RadMenuItem>
                                <telerik:RadMenuItem runat="server" PostBack="False" Text="Rename" Value="2">
                                </telerik:RadMenuItem>
                                <telerik:RadMenuItem runat="server" PostBack="False" Text="Delete" Value="1">
                                </telerik:RadMenuItem>
                            </Items>
                        </telerik:RadTreeViewContextMenu>
                        <telerik:RadTreeViewContextMenu ID="ContextMenuRoot" runat="server">
                            <Items>
                                <telerik:RadMenuItem runat="server" PostBack="False" Text="New" Value="4">
                                </telerik:RadMenuItem>
                                <telerik:RadMenuItem runat="server" PostBack="False" Text="Upload" Value="5">
                                </telerik:RadMenuItem>
                            </Items>
                        </telerik:RadTreeViewContextMenu>
                    </ContextMenus>
                </telerik:RadTreeView>
            </p>
        </div>
        <div class="col-md-8 col-sm-8">
            <h3>Info</h3>
            <div>
                <div>Meta-data: <asp:Label ID="meta" ClientIDMode="Static" runat="server" Text="0"></asp:Label></div>
            </div>
            <input id="fileUpload" type="file" style="display: none;"/>
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

                $("#fileUpload").change(function () { uploadFile(demo.treeView.get_selectedNode()); });
            });

        window.addNode = function (currentNode, id, text, isFolder) {

            var treeView = demo.treeView;
            treeView.trackChanges();

            var node = new Telerik.Web.UI.RadTreeNode();
            node.set_text(text);

            var img = isFolder ? folderImg : fileImg;

            node.set_contextMenuID(isFolder ? 'ContextMenuFolder' : 'ContextMenuFile');

            node.set_imageUrl(img);

            var parent = treeView.get_selectedNode() || treeView;

            if (currentNode == null) {
                parent.get_nodes().add(node);
            } else {
                currentNode.get_nodes().add(node);
            }

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

            node.select(node.get_toolTip());

            if (operation == 6) {
                downloadFile(node.get_toolTip());
                return;
            }

            if (operation == 5) {
                uploadFile(node);
                return;
            }
                
            var newName = null;

            if (operation == 2) {
                newName = prompt('Please enter a new name', node.get_text()).trim();
                if (newName == '')
                    return;
                node.set_text(newName);
            }
            else if (operation == 4) {
                newName = prompt('Please enter a name', 'New folder').trim();
                if (newName == '')
                    return;
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
            onNodeExpanded(sender, args);
        }

        function onNodeExpanded(sender, args) {
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
                success: function (response) {
                    //alert("Success: " + response.d);
                    if (isFolder || operation != 2)
                        setChildren(node, JSON.parse(response.d));
                    if (operation == 3) {
                        var parent = node.get_parent();
                        parent.get_nodes().clear();
                        parent.select();
                        request(0, parent, isFolder, parent.get_toolTip(), null);
                    }
                    if (operation == 4) {
                        node.get_nodes().clear();
                        node.select();
                        request(0, node, isFolder, node.get_toolTip(), null);
                    }
                },
                error: function(xhr, textStatus, error) {
                    alert("Error: " + error);
                }
            });
        }

        function downloadFile(path) {

            path = path.replace(/\\/g, '|');

            $.ajax({
                type: "POST",
                //dataType: "json",
                contentType: "application/json; charset=utf-8",
                url: "Default.aspx/DownloadFile",
                data: "{'path' :'" + path + "'}",

                success: function (response, status, xhr) {
                    var cnt = xhr.getResponseHeader('Content-Disposition');
                    var filename = cnt.split('=')[1];
                    var type = xhr.getResponseHeader('Content-Type');
                    var blob = new Blob([response], { type: type });
                    var url = window.URL || window.webkitURL;
                    var downloadUrl = url.createObjectURL(blob);
                    var a = document.createElement("a");
                    a.href = downloadUrl;
                    a.download = filename;
                    document.body.appendChild(a);
                    a.click();
                },
                error: function (xhr, textStatus, error) {
                    alert("Error: " + error);
                }
            });
        }

        function uploadFile(node) {

            var path = node.get_toolTip().replace(/\\/g, '|');

            var upload = $("#fileUpload");

            var file = upload.get(0).files[0];

            if (file == undefined) {
                upload.trigger('click');
                file = upload.get(0).files[0];
                if (file == undefined)
                    return;
            }

            upload.val('');

            var r = new FileReader();

            r.onload = function () {
                var binimage = r.result;
                $.ajax({
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    url: "Default.aspx/UploadFile",
                    data: "{ 'path': '" + path + "', 'fileName': '" + file.name + "','based64BinaryString' :'" + binimage + "'}",
                    dataType: "json",
                    success: function (response) {
                        node.get_nodes().clear();
                        request(0, node, true, node.get_toolTip(), null);
                    },
                    error: function (xhr, textStatus, error) {
                        alert("Error: " + error);
                    }
                });
            };
            r.readAsDataURL(file);
        }

    </script>

</asp:Content>

