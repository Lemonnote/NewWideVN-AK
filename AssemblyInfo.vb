Imports System
Imports System.Reflection
Imports System.Runtime.InteropServices

'Assembly info / properties
<Assembly: ComVisibleAttribute(False)> 

'The following GUID is for the ID of the typelib if this project is exposed to COM
<Assembly: GuidAttribute("c7a7269f-e825-4aa2-aa88-1460d9e4435f")>

<Assembly: AssemblyTitleAttribute("AK838 for 越南旭榮")>
<Assembly: AssemblyDescriptionAttribute("")> 
<Assembly: AssemblyCompanyAttribute("Adaptive Control")>
<Assembly: AssemblyProductAttribute("AK838 for 越南旭榮")>
<Assembly: AssemblyCopyrightAttribute("Copyright ©  2008")> 
<Assembly: AssemblyTrademarkAttribute("")>

<Assembly: AssemblyVersionAttribute("1.3.*")>
<Assembly: AssemblyFileVersionAttribute("1.3")>

'Version 1.3 2016-08-27 GeorgeLin
'移除不必要的參數

'Version 1.2 2016-08-26 GeorgeLin
'修正使用熱水降溫參數沒有作用的問題

'Version 1.1 2016-07-26 GeorgeLin
'取消pH控制的功能，僅保留pH檢測

'Version 1.0 2016-04-25 GeorgeLin
'  亞磯838 for 越南旭榮第一版程式，修改自標準版