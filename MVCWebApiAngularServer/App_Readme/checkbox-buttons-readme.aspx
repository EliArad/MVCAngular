<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="checkbox-buttons-readme.aspx.cs" Inherits="Checkbox.Css.Buttons.App_Readme.checkbox_buttons_readme" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="~/Content/checkbox-buttons.min.css" rel="stylesheet" />
    <style>
        body
        {
            font-family: Arial, 'DejaVu Sans', 'Liberation Sans', Freesans, sans-serif;
        }
    </style>
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <meta name="description" content="Read Me page to describe the use of Checkbox Buttons package." />
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePartialRendering="false" AsyncPostBackTimeout="36000">
            <Scripts>
                <asp:ScriptReference Path="~/Scripts/jquery-1.9.1.min.js" />
                <asp:ScriptReference Path="~/Scripts/nac-checkbox-buttons.js" />
            </Scripts>
        </asp:ScriptManager>
        <div>
            <h1>Checkbox and Radio Buttons to Button groups</h1>
            <p>
                This makes Asp.Net CheckBox and CheckboxList look like buttons, 
                also work for none Asp.Net checkboxes look like buttons similar to 
                <a href="http://getbootstrap.com/2.3.2/javascript.html#buttons">Bootstrap JavaScript Buttons</a>
                and allows the button to show checkboxes or radios button to the right now.
            </p>
            <p>
                Simply add the 'checkbox-buttons' 
                css class to the CheckboxList or to a span containing some checkboxes and there you go. 
                With the 'show-checkbox' css class you will need to add the 'nac-checkbox-buttons' JavaScript file (For WebForms only) 
                to move the checkbox into the label for that to work.
            </p>
            <p>
                You can also add the 'with-border-radius' css class to set the ends to rounded corners.
            </p>
            <div>
                <h2>Checkboxes</h2>
                <h3>WebForms</h3>
                <h4>Rounded Corners</h4>
                <asp:CheckBoxList ID="CheckBoxList3" runat="server" CssClass="checkbox-buttons with-border-radius" RepeatDirection="Horizontal" RepeatLayout="Flow">
                    <asp:ListItem>Test A</asp:ListItem>
                    <asp:ListItem>Test B</asp:ListItem>
                    <asp:ListItem>Test C</asp:ListItem>
                    <asp:ListItem>Test D</asp:ListItem>
                </asp:CheckBoxList>
                <br />
                <asp:CheckBoxList ID="CheckBoxList4" runat="server" CssClass="checkbox-buttons show-checkbox with-border-radius" RepeatDirection="Horizontal" RepeatLayout="Flow">
                    <asp:ListItem>Test A</asp:ListItem>
                    <asp:ListItem>Test B</asp:ListItem>
                    <asp:ListItem>Test C</asp:ListItem>
                    <asp:ListItem>Test D</asp:ListItem>
                </asp:CheckBoxList>

                <h4>Square Corners</h4>
                <asp:CheckBoxList ID="CheckBoxList5" runat="server" CssClass="checkbox-buttons" RepeatDirection="Horizontal" RepeatLayout="Flow">
                    <asp:ListItem>Test A</asp:ListItem>
                    <asp:ListItem>Test B</asp:ListItem>
                    <asp:ListItem>Test C</asp:ListItem>
                    <asp:ListItem>Test D</asp:ListItem>
                </asp:CheckBoxList>
                <br />
                <asp:CheckBoxList ID="CheckBoxList6" runat="server" CssClass="checkbox-buttons show-checkbox" RepeatDirection="Horizontal" RepeatLayout="Flow">
                    <asp:ListItem>Test A</asp:ListItem>
                    <asp:ListItem>Test B</asp:ListItem>
                    <asp:ListItem>Test C</asp:ListItem>
                    <asp:ListItem>Test D</asp:ListItem>
                </asp:CheckBoxList>
                <hr />

                <h3>Plain HTML</h3>
                <h4>Rounded Corners</h4>
                <div id="CheckBoxList1" class="checkbox-buttons with-border-radius">
                    <input id="CheckBoxList1_0" type="checkbox" name="CheckBoxList1$0" value="A" /><label for="CheckBoxList1_0">A</label><input id="CheckBoxList1_1" type="checkbox" name="CheckBoxList1$1" value="B" /><label for="CheckBoxList1_1">B</label><input id="CheckBoxList1_2" type="checkbox" name="CheckBoxList1$2" value="C" /><label for="CheckBoxList1_2">C</label>
                </div>

                <div id="CheckBoxList2" class="checkbox-buttons show-checkbox with-border-radius">
                    <input id="CheckBoxList2_0" type="checkbox" name="CheckBoxList2$0" value="A" /><label for="CheckBoxList2_0">A</label><input id="CheckBoxList2_1" type="checkbox" name="CheckBoxList2$1" value="B" /><label for="CheckBoxList2_1">B</label><input id="CheckBoxList2_2" type="checkbox" name="CheckBoxList2$2" value="C" /><label for="CheckBoxList2_2">C</label>
                </div>

                <h4>Square Corners</h4>
                <div class="checkbox-buttons">
                    <input id="CheckBox1" type="checkbox" name="CheckBox1" /><label for="CheckBox1">Test 1</label><input id="CheckBox2" type="checkbox" name="CheckBox2" /><label for="CheckBox2">Test 2</label><input id="CheckBox3" type="checkbox" name="CheckBox3" /><label for="CheckBox3">Test 3</label>
                </div>

                <div class="checkbox-buttons show-checkbox">
                    <input id="CheckBox4" type="checkbox" name="CheckBox4" /><label for="CheckBox4">Test 4</label><input id="CheckBox5" type="checkbox" name="CheckBox5" /><label for="CheckBox5">Test 5</label><input id="CheckBox6" type="checkbox" name="CheckBox6" /><label for="CheckBox6">Test 6</label>
                </div>
                <hr />

                <h2>Radio Buttons</h2>
                <h3>WebForms</h3>
                <h4>Rounded Corners</h4>
                <asp:RadioButtonList ID="RadioButtonList1" runat="server" CssClass="checkbox-buttons with-border-radius" RepeatDirection="Horizontal" RepeatLayout="Flow">
                    <asp:ListItem>Test A</asp:ListItem>
                    <asp:ListItem>Test B</asp:ListItem>
                    <asp:ListItem>Test C</asp:ListItem>
                    <asp:ListItem>Test D</asp:ListItem>
                </asp:RadioButtonList>
                <br />
                <asp:RadioButtonList ID="RadioButtonList2" runat="server" CssClass="checkbox-buttons with-border-radius show-checkbox" RepeatDirection="Horizontal" RepeatLayout="Flow">
                    <asp:ListItem>Test A</asp:ListItem>
                    <asp:ListItem>Test B</asp:ListItem>
                    <asp:ListItem>Test C</asp:ListItem>
                    <asp:ListItem>Test D</asp:ListItem>
                </asp:RadioButtonList>

                <h4>Square Corners</h4>
                <asp:RadioButtonList ID="RadioButtonList3" runat="server" CssClass="checkbox-buttons" RepeatDirection="Horizontal" RepeatLayout="Flow">
                    <asp:ListItem>Test A</asp:ListItem>
                    <asp:ListItem>Test B</asp:ListItem>
                    <asp:ListItem>Test C</asp:ListItem>
                    <asp:ListItem>Test D</asp:ListItem>
                </asp:RadioButtonList>
                <br />
                <asp:RadioButtonList ID="RadioButtonList4" runat="server" CssClass="checkbox-buttons show-checkbox" RepeatDirection="Horizontal" RepeatLayout="Flow">
                    <asp:ListItem>Test A</asp:ListItem>
                    <asp:ListItem>Test B</asp:ListItem>
                    <asp:ListItem>Test C</asp:ListItem>
                    <asp:ListItem>Test D</asp:ListItem>
                </asp:RadioButtonList>
                <hr />

                <h3>Plain HTML</h3>
                <h4>Rounded Corners</h4>
                <div id="RadioButtonList5" class="checkbox-buttons with-border-radius">
                    <input id="RadioButtonList5_0" type="radio" name="RadioButtonList5" value="1" /><label for="RadioButtonList5_0">1</label><input id="RadioButtonList5_1" type="radio" name="RadioButtonList5" value="2" /><label for="RadioButtonList5_1">2</label><input id="RadioButtonList5_2" type="radio" name="RadioButtonList5" value="3" /><label for="RadioButtonList5_2">3</label>
                </div>
                <div id="RadioButtonList6" class="checkbox-buttons show-checkbox with-border-radius">
                    <input id="RadioButtonList6_0" type="radio" name="RadioButtonList6" value="1" /><label for="RadioButtonList6_0">1</label><input id="RadioButtonList6_1" type="radio" name="RadioButtonList6" value="2" /><label for="RadioButtonList6_1">2</label><input id="RadioButtonList6_2" type="radio" name="RadioButtonList6" value="3" /><label for="RadioButtonList6_2">3</label>
                </div>
                <h4>Square Corners</h4>
                <div id="RadioButtonList7" class="checkbox-buttons">
                    <input id="RadioButtonList7_0" type="radio" name="RadioButtonList7" value="1" /><label for="RadioButtonList7_0">1</label><input id="RadioButtonList7_1" type="radio" name="RadioButtonList7" value="2" /><label for="RadioButtonList7_1">2</label><input id="RadioButtonList7_2" type="radio" name="RadioButtonList7" value="3" /><label for="RadioButtonList7_2">3</label>
                </div>
                <div id="RadioButtonList8" class="checkbox-buttons show-checkbox">
                    <input id="RadioButtonList8_0" type="radio" name="RadioButtonList8" value="1" /><label for="RadioButtonList8_0">1</label><input id="RadioButtonList8_1" type="radio" name="RadioButtonList8" value="2" /><label for="RadioButtonList8_1">2</label><input id="RadioButtonList8_2" type="radio" name="RadioButtonList8" value="3" /><label for="RadioButtonList8_2">3</label>
                </div>
            </div>
        </div>
        <%--<script src="../Scripts/nac-checkbox-buttons.js"></script>--%>
    </form>
</body>
</html>
