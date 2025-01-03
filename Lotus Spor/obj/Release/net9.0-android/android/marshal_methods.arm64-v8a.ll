; ModuleID = 'marshal_methods.arm64-v8a.ll'
source_filename = "marshal_methods.arm64-v8a.ll"
target datalayout = "e-m:e-i8:8:32-i16:16:32-i64:64-i128:128-n32:64-S128"
target triple = "aarch64-unknown-linux-android21"

%struct.MarshalMethodName = type {
	i64, ; uint64_t id
	ptr ; char* name
}

%struct.MarshalMethodsManagedClass = type {
	i32, ; uint32_t token
	ptr ; MonoClass klass
}

@assembly_image_cache = dso_local local_unnamed_addr global [219 x ptr] zeroinitializer, align 8

; Each entry maps hash of an assembly name to an index into the `assembly_image_cache` array
@assembly_image_cache_hashes = dso_local local_unnamed_addr constant [657 x i64] [
	i64 u0x006b9d7c1c7e1c42, ; 0: de/Microsoft.Data.SqlClient.resources => 0
	i64 u0x0071cf2d27b7d61e, ; 1: lib_Xamarin.AndroidX.SwipeRefreshLayout.dll.so => 116
	i64 u0x01109b0e4d99e61f, ; 2: System.ComponentModel.Annotations.dll => 133
	i64 u0x02123411c4e01926, ; 3: lib_Xamarin.AndroidX.Navigation.Runtime.dll.so => 112
	i64 u0x029b2c18aaa0996c, ; 4: lib-ko-Microsoft.Data.SqlClient.resources.dll.so => 5
	i64 u0x02a4c5a44384f885, ; 5: Microsoft.Extensions.Caching.Memory => 58
	i64 u0x02abedc11addc1ed, ; 6: lib_Mono.Android.Runtime.dll.so => 217
	i64 u0x032267b2a94db371, ; 7: lib_Xamarin.AndroidX.AppCompat.dll.so => 94
	i64 u0x03621c804933a890, ; 8: System.Buffers => 127
	i64 u0x0399610510a38a38, ; 9: lib_System.Private.DataContractSerialization.dll.so => 170
	i64 u0x043032f1d071fae0, ; 10: ru/Microsoft.Maui.Controls.resources => 34
	i64 u0x044440a55165631e, ; 11: lib-cs-Microsoft.Maui.Controls.resources.dll.so => 12
	i64 u0x046eb1581a80c6b0, ; 12: vi/Microsoft.Maui.Controls.resources => 40
	i64 u0x0470607fd33c32db, ; 13: Microsoft.IdentityModel.Abstractions.dll => 69
	i64 u0x0517ef04e06e9f76, ; 14: System.Net.Primitives => 162
	i64 u0x0565d18c6da3de38, ; 15: Xamarin.AndroidX.RecyclerView => 114
	i64 u0x0581db89237110e9, ; 16: lib_System.Collections.dll.so => 132
	i64 u0x05989cb940b225a9, ; 17: Microsoft.Maui.dll => 77
	i64 u0x05a1c25e78e22d87, ; 18: lib_System.Runtime.CompilerServices.Unsafe.dll.so => 177
	i64 u0x05d8ca8ee551619f, ; 19: zh-Hant/Microsoft.Data.SqlClient.resources => 9
	i64 u0x06076b5d2b581f08, ; 20: zh-HK/Microsoft.Maui.Controls.resources => 41
	i64 u0x06388ffe9f6c161a, ; 21: System.Xml.Linq.dll => 209
	i64 u0x0680a433c781bb3d, ; 22: Xamarin.AndroidX.Collection.Jvm => 98
	i64 u0x07c57877c7ba78ad, ; 23: ru/Microsoft.Maui.Controls.resources.dll => 34
	i64 u0x07dcdc7460a0c5e4, ; 24: System.Collections.NonGeneric => 130
	i64 u0x08015600dcbf6dc7, ; 25: it/Microsoft.Data.SqlClient.resources.dll => 3
	i64 u0x08881a0a9768df86, ; 26: lib_Azure.Core.dll.so => 45
	i64 u0x08a7c865576bbde7, ; 27: System.Reflection.Primitives => 176
	i64 u0x08f3c9788ee2153c, ; 28: Xamarin.AndroidX.DrawerLayout => 103
	i64 u0x09138715c92dba90, ; 29: lib_System.ComponentModel.Annotations.dll.so => 133
	i64 u0x0919c28b89381a0b, ; 30: lib_Microsoft.Extensions.Options.dll.so => 65
	i64 u0x092266563089ae3e, ; 31: lib_System.Collections.NonGeneric.dll.so => 130
	i64 u0x095cacaf6b6a32e4, ; 32: System.Memory.Data => 88
	i64 u0x09d144a7e214d457, ; 33: System.Security.Cryptography => 196
	i64 u0x0a805f95d98f597b, ; 34: lib_Microsoft.Extensions.Caching.Abstractions.dll.so => 57
	i64 u0x0abb3e2b271edc45, ; 35: System.Threading.Channels.dll => 202
	i64 u0x0adeb6c0f5699d33, ; 36: Microsoft.Data.SqlClient.dll => 53
	i64 u0x0b3b632c3bbee20c, ; 37: sk/Microsoft.Maui.Controls.resources => 35
	i64 u0x0b6aff547b84fbe9, ; 38: Xamarin.KotlinX.Serialization.Core.Jvm => 122
	i64 u0x0be2e1f8ce4064ed, ; 39: Xamarin.AndroidX.ViewPager => 117
	i64 u0x0c3ca6cc978e2aae, ; 40: pt-BR/Microsoft.Maui.Controls.resources => 31
	i64 u0x0c59ad9fbbd43abe, ; 41: Mono.Android => 218
	i64 u0x0c7790f60165fc06, ; 42: lib_Microsoft.Maui.Essentials.dll.so => 78
	i64 u0x0d3b5ab8b2766190, ; 43: lib_Microsoft.Bcl.AsyncInterfaces.dll.so => 52
	i64 u0x0e14e73a54dda68e, ; 44: lib_System.Net.NameResolution.dll.so => 160
	i64 u0x0fbe06392ef90569, ; 45: lib-ja-Microsoft.Data.SqlClient.resources.dll.so => 4
	i64 u0x102861e4055f511a, ; 46: Microsoft.Bcl.AsyncInterfaces.dll => 52
	i64 u0x102a31b45304b1da, ; 47: Xamarin.AndroidX.CustomView => 102
	i64 u0x108cf0e0ba098a51, ; 48: es/Microsoft.Data.SqlClient.resources => 1
	i64 u0x10f6cfcbcf801616, ; 49: System.IO.Compression.Brotli => 149
	i64 u0x114443cdcf2091f1, ; 50: System.Security.Cryptography.Primitives => 194
	i64 u0x123639456fb056da, ; 51: System.Reflection.Emit.Lightweight.dll => 175
	i64 u0x124f38a5d8cb5fb8, ; 52: K4os.Compression.LZ4.dll => 49
	i64 u0x125b7f94acb989db, ; 53: Xamarin.AndroidX.RecyclerView.dll => 114
	i64 u0x126ee4b0de53cbfd, ; 54: Microsoft.IdentityModel.Protocols.OpenIdConnect.dll => 73
	i64 u0x138567fa954faa55, ; 55: Xamarin.AndroidX.Browser => 96
	i64 u0x13a01de0cbc3f06c, ; 56: lib-fr-Microsoft.Maui.Controls.resources.dll.so => 18
	i64 u0x13f1e5e209e91af4, ; 57: lib_Java.Interop.dll.so => 216
	i64 u0x13f1e880c25d96d1, ; 58: he/Microsoft.Maui.Controls.resources => 19
	i64 u0x143a1f6e62b82b56, ; 59: Microsoft.IdentityModel.Protocols.OpenIdConnect => 73
	i64 u0x143d8ea60a6a4011, ; 60: Microsoft.Extensions.DependencyInjection.Abstractions => 62
	i64 u0x152a448bd1e745a7, ; 61: Microsoft.Win32.Primitives => 126
	i64 u0x159cc6c81072f00e, ; 62: lib_System.Diagnostics.EventLog.dll.so => 86
	i64 u0x162be8a76b00cd97, ; 63: lib-de-Microsoft.Data.SqlClient.resources.dll.so => 0
	i64 u0x16bf2a22df043a09, ; 64: System.IO.Pipes.dll => 154
	i64 u0x16ea2b318ad2d830, ; 65: System.Security.Cryptography.Algorithms => 190
	i64 u0x17125c9a85b4929f, ; 66: lib_netstandard.dll.so => 214
	i64 u0x1716866f7416792e, ; 67: lib_System.Security.AccessControl.dll.so => 188
	i64 u0x17b56e25558a5d36, ; 68: lib-hu-Microsoft.Maui.Controls.resources.dll.so => 22
	i64 u0x17f9358913beb16a, ; 69: System.Text.Encodings.Web => 200
	i64 u0x18402a709e357f3b, ; 70: lib_Xamarin.KotlinX.Serialization.Core.Jvm.dll.so => 122
	i64 u0x18a9befae51bb361, ; 71: System.Net.WebClient => 166
	i64 u0x18f0ce884e87d89a, ; 72: nb/Microsoft.Maui.Controls.resources.dll => 28
	i64 u0x19a4c090f14ebb66, ; 73: System.Security.Claims => 189
	i64 u0x1a6fceea64859810, ; 74: Azure.Identity => 46
	i64 u0x1a91866a319e9259, ; 75: lib_System.Collections.Concurrent.dll.so => 128
	i64 u0x1aac34d1917ba5d3, ; 76: lib_System.dll.so => 213
	i64 u0x1aad60783ffa3e5b, ; 77: lib-th-Microsoft.Maui.Controls.resources.dll.so => 37
	i64 u0x1c753b5ff15bce1b, ; 78: Mono.Android.Runtime.dll => 217
	i64 u0x1db6820994506bf5, ; 79: System.IO.FileSystem.AccessControl.dll => 151
	i64 u0x1dba6509cc55b56f, ; 80: lib_Google.Protobuf.dll.so => 48
	i64 u0x1dbb0c2c6a999acb, ; 81: System.Diagnostics.StackTrace => 142
	i64 u0x1e3d87657e9659bc, ; 82: Xamarin.AndroidX.Navigation.UI => 113
	i64 u0x1e71143913d56c10, ; 83: lib-ko-Microsoft.Maui.Controls.resources.dll.so => 26
	i64 u0x1ed8fcce5e9b50a0, ; 84: Microsoft.Extensions.Options.dll => 65
	i64 u0x1f055d15d807e1b2, ; 85: System.Xml.XmlSerializer => 212
	i64 u0x20237ea48006d7a8, ; 86: lib_System.Net.WebClient.dll.so => 166
	i64 u0x209375905fcc1bad, ; 87: lib_System.IO.Compression.Brotli.dll.so => 149
	i64 u0x20edad43b59fbd8e, ; 88: System.Security.Permissions.dll => 91
	i64 u0x20fab3cf2dfbc8df, ; 89: lib_System.Diagnostics.Process.dll.so => 141
	i64 u0x2174319c0d835bc9, ; 90: System.Runtime => 187
	i64 u0x2199f06354c82d3b, ; 91: System.ClientModel.dll => 83
	i64 u0x21cc7e445dcd5469, ; 92: System.Reflection.Emit.ILGeneration => 174
	i64 u0x220fd4f2e7c48170, ; 93: th/Microsoft.Maui.Controls.resources => 37
	i64 u0x224538d85ed15a82, ; 94: System.IO.Pipes => 154
	i64 u0x234b2420fe4b9bdc, ; 95: lib_K4os.Compression.LZ4.dll.so => 49
	i64 u0x237be844f1f812c7, ; 96: System.Threading.Thread.dll => 204
	i64 u0x23807c59646ec4f3, ; 97: lib_Microsoft.EntityFrameworkCore.dll.so => 54
	i64 u0x23cce13de11e9adc, ; 98: Lotus Spor.dll => 124
	i64 u0x2407aef2bbe8fadf, ; 99: System.Console => 138
	i64 u0x240abe014b27e7d3, ; 100: Xamarin.AndroidX.Core.dll => 100
	i64 u0x247619fe4413f8bf, ; 101: System.Runtime.Serialization.Primitives.dll => 185
	i64 u0x252073cc3caa62c2, ; 102: fr/Microsoft.Maui.Controls.resources.dll => 18
	i64 u0x2662c629b96b0b30, ; 103: lib_Xamarin.Kotlin.StdLib.dll.so => 120
	i64 u0x268c1439f13bcc29, ; 104: lib_Microsoft.Extensions.Primitives.dll.so => 66
	i64 u0x270a44600c921861, ; 105: System.IdentityModel.Tokens.Jwt => 87
	i64 u0x273f3515de5faf0d, ; 106: id/Microsoft.Maui.Controls.resources.dll => 23
	i64 u0x2742545f9094896d, ; 107: hr/Microsoft.Maui.Controls.resources => 21
	i64 u0x27b410442fad6cf1, ; 108: Java.Interop.dll => 216
	i64 u0x2801845a2c71fbfb, ; 109: System.Net.Primitives.dll => 162
	i64 u0x28e27b6a43116264, ; 110: lib_Lotus Spor.dll.so => 124
	i64 u0x2a128783efe70ba0, ; 111: uk/Microsoft.Maui.Controls.resources.dll => 39
	i64 u0x2a3b095612184159, ; 112: lib_System.Net.NetworkInformation.dll.so => 161
	i64 u0x2a6507a5ffabdf28, ; 113: System.Diagnostics.TraceSource.dll => 144
	i64 u0x2ad156c8e1354139, ; 114: fi/Microsoft.Maui.Controls.resources => 17
	i64 u0x2af298f63581d886, ; 115: System.Text.RegularExpressions.dll => 201
	i64 u0x2af615542f04da50, ; 116: System.IdentityModel.Tokens.Jwt.dll => 87
	i64 u0x2afc1c4f898552ee, ; 117: lib_System.Formats.Asn1.dll.so => 148
	i64 u0x2b148910ed40fbf9, ; 118: zh-Hant/Microsoft.Maui.Controls.resources.dll => 43
	i64 u0x2c8bd14bb93a7d82, ; 119: lib-pl-Microsoft.Maui.Controls.resources.dll.so => 30
	i64 u0x2cbd9262ca785540, ; 120: lib_System.Text.Encoding.CodePages.dll.so => 198
	i64 u0x2cc9e1fed6257257, ; 121: lib_System.Reflection.Emit.Lightweight.dll.so => 175
	i64 u0x2cd723e9fe623c7c, ; 122: lib_System.Private.Xml.Linq.dll.so => 172
	i64 u0x2ce66f4c8733e883, ; 123: pt-BR/Microsoft.Data.SqlClient.resources.dll => 6
	i64 u0x2d169d318a968379, ; 124: System.Threading.dll => 206
	i64 u0x2d47774b7d993f59, ; 125: sv/Microsoft.Maui.Controls.resources.dll => 36
	i64 u0x2db915caf23548d2, ; 126: System.Text.Json.dll => 92
	i64 u0x2e6f1f226821322a, ; 127: el/Microsoft.Maui.Controls.resources.dll => 15
	i64 u0x2f02f94df3200fe5, ; 128: System.Diagnostics.Process => 141
	i64 u0x2f2e98e1c89b1aff, ; 129: System.Xml.ReaderWriter => 210
	i64 u0x2f40b2521deba305, ; 130: lib_Microsoft.SqlServer.Server.dll.so => 80
	i64 u0x2f5911d9ba814e4e, ; 131: System.Diagnostics.Tracing => 145
	i64 u0x2feb4d2fcda05cfd, ; 132: Microsoft.Extensions.Caching.Abstractions.dll => 57
	i64 u0x309ee9eeec09a71e, ; 133: lib_Xamarin.AndroidX.Fragment.dll.so => 104
	i64 u0x309f2bedefa9a318, ; 134: Microsoft.IdentityModel.Abstractions => 69
	i64 u0x31195fef5d8fb552, ; 135: _Microsoft.Android.Resource.Designer.dll => 44
	i64 u0x31a267c1617036d6, ; 136: MySql.EntityFrameworkCore.dll => 82
	i64 u0x32243413e774362a, ; 137: Xamarin.AndroidX.CardView.dll => 97
	i64 u0x3235427f8d12dae1, ; 138: lib_System.Drawing.Primitives.dll.so => 146
	i64 u0x329753a17a517811, ; 139: fr/Microsoft.Maui.Controls.resources => 18
	i64 u0x32aa989ff07a84ff, ; 140: lib_System.Xml.ReaderWriter.dll.so => 210
	i64 u0x33829542f112d59b, ; 141: System.Collections.Immutable => 129
	i64 u0x33a31443733849fe, ; 142: lib-es-Microsoft.Maui.Controls.resources.dll.so => 16
	i64 u0x341abc357fbb4ebf, ; 143: lib_System.Net.Sockets.dll.so => 165
	i64 u0x348d598f4054415e, ; 144: Microsoft.SqlServer.Server => 80
	i64 u0x34dfd74fe2afcf37, ; 145: Microsoft.Maui => 77
	i64 u0x34e292762d9615df, ; 146: cs/Microsoft.Maui.Controls.resources.dll => 12
	i64 u0x3508234247f48404, ; 147: Microsoft.Maui.Controls => 75
	i64 u0x353590da528c9d22, ; 148: System.ComponentModel.Annotations => 133
	i64 u0x3549870798b4cd30, ; 149: lib_Xamarin.AndroidX.ViewPager2.dll.so => 118
	i64 u0x355282fc1c909694, ; 150: Microsoft.Extensions.Configuration => 59
	i64 u0x355c649948d55d97, ; 151: lib_System.Runtime.Intrinsics.dll.so => 180
	i64 u0x36b2b50fdf589ae2, ; 152: System.Reflection.Emit.Lightweight => 175
	i64 u0x374ef46b06791af6, ; 153: System.Reflection.Primitives.dll => 176
	i64 u0x380134e03b1e160a, ; 154: System.Collections.Immutable.dll => 129
	i64 u0x38049b5c59b39324, ; 155: System.Runtime.CompilerServices.Unsafe => 177
	i64 u0x385c17636bb6fe6e, ; 156: Xamarin.AndroidX.CustomView.dll => 102
	i64 u0x38869c811d74050e, ; 157: System.Net.NameResolution.dll => 160
	i64 u0x38e93ec1c057cdf6, ; 158: Microsoft.IdentityModel.Protocols => 72
	i64 u0x39251dccb84bdcaa, ; 159: lib_System.Configuration.ConfigurationManager.dll.so => 84
	i64 u0x393c226616977fdb, ; 160: lib_Xamarin.AndroidX.ViewPager.dll.so => 117
	i64 u0x395e37c3334cf82a, ; 161: lib-ca-Microsoft.Maui.Controls.resources.dll.so => 11
	i64 u0x3ab5859054645f72, ; 162: System.Security.Cryptography.Primitives.dll => 194
	i64 u0x3b2c47fe17204e4d, ; 163: MySql.Data => 81
	i64 u0x3b860f9932505633, ; 164: lib_System.Text.Encoding.Extensions.dll.so => 199
	i64 u0x3bea9ebe8c027c01, ; 165: lib_Microsoft.IdentityModel.Tokens.dll.so => 74
	i64 u0x3c3aafb6b3a00bf6, ; 166: lib_System.Security.Cryptography.X509Certificates.dll.so => 195
	i64 u0x3c5f19e4acdcebd8, ; 167: lib_Microsoft.Data.SqlClient.dll.so => 53
	i64 u0x3c7c495f58ac5ee9, ; 168: Xamarin.Kotlin.StdLib => 120
	i64 u0x3cd9d281d402eb9b, ; 169: Xamarin.AndroidX.Browser.dll => 96
	i64 u0x3d196e782ed8c01a, ; 170: System.Data.SqlClient => 85
	i64 u0x3d2b1913edfc08d7, ; 171: lib_System.Threading.ThreadPool.dll.so => 205
	i64 u0x3d46f0b995082740, ; 172: System.Xml.Linq => 209
	i64 u0x3d9c2a242b040a50, ; 173: lib_Xamarin.AndroidX.Core.dll.so => 100
	i64 u0x3daa14724d8f58e8, ; 174: Google.Protobuf.dll => 48
	i64 u0x3e0b360b2840f096, ; 175: it/Microsoft.Data.SqlClient.resources => 3
	i64 u0x3f3c8f45ab6f28c7, ; 176: Microsoft.Identity.Client.Extensions.Msal.dll => 68
	i64 u0x3f510adf788828dd, ; 177: System.Threading.Tasks.Extensions => 203
	i64 u0x4019503dd3d938a1, ; 178: MySql.Data.dll => 81
	i64 u0x407a10bb4bf95829, ; 179: lib_Xamarin.AndroidX.Navigation.Common.dll.so => 110
	i64 u0x407ac43dee26bd5a, ; 180: lib_Azure.Identity.dll.so => 46
	i64 u0x415e36f6b13ff6f3, ; 181: System.Configuration.ConfigurationManager.dll => 84
	i64 u0x41cab042be111c34, ; 182: lib_Xamarin.AndroidX.AppCompat.AppCompatResources.dll.so => 95
	i64 u0x43375950ec7c1b6a, ; 183: netstandard.dll => 214
	i64 u0x434c4e1d9284cdae, ; 184: Mono.Android.dll => 218
	i64 u0x43950f84de7cc79a, ; 185: pl/Microsoft.Maui.Controls.resources.dll => 30
	i64 u0x448bd33429269b19, ; 186: Microsoft.CSharp => 125
	i64 u0x4499fa3c8e494654, ; 187: lib_System.Runtime.Serialization.Primitives.dll.so => 185
	i64 u0x4515080865a951a5, ; 188: Xamarin.Kotlin.StdLib.dll => 120
	i64 u0x453c1277f85cf368, ; 189: lib_Microsoft.EntityFrameworkCore.Abstractions.dll.so => 55
	i64 u0x458d2df79ac57c1d, ; 190: lib_System.IdentityModel.Tokens.Jwt.dll.so => 87
	i64 u0x45c40276a42e283e, ; 191: System.Diagnostics.TraceSource => 144
	i64 u0x46a4213bc97fe5ae, ; 192: lib-ru-Microsoft.Maui.Controls.resources.dll.so => 34
	i64 u0x47358bd471172e1d, ; 193: lib_System.Xml.Linq.dll.so => 209
	i64 u0x4787a936949fcac2, ; 194: System.Memory.Data.dll => 88
	i64 u0x47daf4e1afbada10, ; 195: pt/Microsoft.Maui.Controls.resources => 32
	i64 u0x4953c088b9debf0a, ; 196: lib_System.Security.Permissions.dll.so => 91
	i64 u0x49e952f19a4e2022, ; 197: System.ObjectModel => 169
	i64 u0x4a5667b2462a664b, ; 198: lib_Xamarin.AndroidX.Navigation.UI.dll.so => 113
	i64 u0x4b576d47ac054f3c, ; 199: System.IO.FileSystem.AccessControl => 151
	i64 u0x4b7b6532ded934b7, ; 200: System.Text.Json => 92
	i64 u0x4b8f8ea3c2df6bb0, ; 201: System.ClientModel => 83
	i64 u0x4ca014ceac582c86, ; 202: Microsoft.EntityFrameworkCore.Relational.dll => 56
	i64 u0x4cc5f15266470798, ; 203: lib_Xamarin.AndroidX.Loader.dll.so => 109
	i64 u0x4cf6f67dc77aacd2, ; 204: System.Net.NetworkInformation.dll => 161
	i64 u0x4d479f968a05e504, ; 205: System.Linq.Expressions.dll => 155
	i64 u0x4d55a010ffc4faff, ; 206: System.Private.Xml => 173
	i64 u0x4d6001db23f8cd87, ; 207: lib_System.ClientModel.dll.so => 83
	i64 u0x4d95fccc1f67c7ca, ; 208: System.Runtime.Loader.dll => 181
	i64 u0x4dcf44c3c9b076a2, ; 209: it/Microsoft.Maui.Controls.resources.dll => 24
	i64 u0x4dd9247f1d2c3235, ; 210: Xamarin.AndroidX.Loader.dll => 109
	i64 u0x4e32f00cb0937401, ; 211: Mono.Android.Runtime => 217
	i64 u0x4e5eea4668ac2b18, ; 212: System.Text.Encoding.CodePages => 198
	i64 u0x4ebd0c4b82c5eefc, ; 213: lib_System.Threading.Channels.dll.so => 202
	i64 u0x4f21ee6ef9eb527e, ; 214: ca/Microsoft.Maui.Controls.resources => 11
	i64 u0x4ffd65baff757598, ; 215: Microsoft.IdentityModel.Tokens => 74
	i64 u0x50320f2a19424f3f, ; 216: lib-it-Microsoft.Data.SqlClient.resources.dll.so => 3
	i64 u0x5037f0be3c28c7a3, ; 217: lib_Microsoft.Maui.Controls.dll.so => 75
	i64 u0x5131bbe80989093f, ; 218: Xamarin.AndroidX.Lifecycle.ViewModel.Android.dll => 107
	i64 u0x5146d4e23aed3198, ; 219: ja/Microsoft.Data.SqlClient.resources => 4
	i64 u0x51bb8a2afe774e32, ; 220: System.Drawing => 147
	i64 u0x526ce79eb8e90527, ; 221: lib_System.Net.Primitives.dll.so => 162
	i64 u0x52829f00b4467c38, ; 222: lib_System.Data.Common.dll.so => 139
	i64 u0x5290402954d7bce0, ; 223: zh-Hans/Microsoft.Data.SqlClient.resources => 8
	i64 u0x529ffe06f39ab8db, ; 224: Xamarin.AndroidX.Core => 100
	i64 u0x52ff996554dbf352, ; 225: Microsoft.Maui.Graphics => 79
	i64 u0x535f7e40e8fef8af, ; 226: lib-sk-Microsoft.Maui.Controls.resources.dll.so => 35
	i64 u0x53978aac584c666e, ; 227: lib_System.Security.Cryptography.Cng.dll.so => 191
	i64 u0x53a96d5c86c9e194, ; 228: System.Net.NetworkInformation => 161
	i64 u0x53be1038a61e8d44, ; 229: System.Runtime.InteropServices.RuntimeInformation.dll => 178
	i64 u0x53c3014b9437e684, ; 230: lib-zh-HK-Microsoft.Maui.Controls.resources.dll.so => 41
	i64 u0x5435e6f049e9bc37, ; 231: System.Security.Claims.dll => 189
	i64 u0x54795225dd1587af, ; 232: lib_System.Runtime.dll.so => 187
	i64 u0x556e8b63b660ab8b, ; 233: Xamarin.AndroidX.Lifecycle.Common.Jvm.dll => 105
	i64 u0x5588627c9a108ec9, ; 234: System.Collections.Specialized => 131
	i64 u0x56442b99bc64bb47, ; 235: System.Runtime.Serialization.Xml.dll => 186
	i64 u0x571c5cfbec5ae8e2, ; 236: System.Private.Uri => 171
	i64 u0x579a06fed6eec900, ; 237: System.Private.CoreLib.dll => 215
	i64 u0x57c542c14049b66d, ; 238: System.Diagnostics.DiagnosticSource => 140
	i64 u0x58601b2dda4a27b9, ; 239: lib-ja-Microsoft.Maui.Controls.resources.dll.so => 25
	i64 u0x58688d9af496b168, ; 240: Microsoft.Extensions.DependencyInjection.dll => 61
	i64 u0x595a356d23e8da9a, ; 241: lib_Microsoft.CSharp.dll.so => 125
	i64 u0x5a70033ca9d003cb, ; 242: lib_System.Memory.Data.dll.so => 88
	i64 u0x5a89a886ae30258d, ; 243: lib_Xamarin.AndroidX.CoordinatorLayout.dll.so => 99
	i64 u0x5a8f6699f4a1caa9, ; 244: lib_System.Threading.dll.so => 206
	i64 u0x5ae9cd33b15841bf, ; 245: System.ComponentModel => 137
	i64 u0x5b54391bdc6fcfe6, ; 246: System.Private.DataContractSerialization => 170
	i64 u0x5b5f0e240a06a2a2, ; 247: da/Microsoft.Maui.Controls.resources.dll => 13
	i64 u0x5b608c01082a90a8, ; 248: K4os.Hash.xxHash => 51
	i64 u0x5bf46332cc09e9b2, ; 249: lib_System.Data.SqlClient.dll.so => 85
	i64 u0x5c393624b8176517, ; 250: lib_Microsoft.Extensions.Logging.dll.so => 63
	i64 u0x5d0a4a29b02d9d3c, ; 251: System.Net.WebHeaderCollection.dll => 167
	i64 u0x5d33da2f84c1de97, ; 252: lib-pt-BR-Microsoft.Data.SqlClient.resources.dll.so => 6
	i64 u0x5db0cbbd1028510e, ; 253: lib_System.Runtime.InteropServices.dll.so => 179
	i64 u0x5db30905d3e5013b, ; 254: Xamarin.AndroidX.Collection.Jvm.dll => 98
	i64 u0x5e467bc8f09ad026, ; 255: System.Collections.Specialized.dll => 131
	i64 u0x5ea92fdb19ec8c4c, ; 256: System.Text.Encodings.Web.dll => 200
	i64 u0x5eb8046dd40e9ac3, ; 257: System.ComponentModel.Primitives => 135
	i64 u0x5ec272d219c9aba4, ; 258: System.Security.Cryptography.Csp.dll => 192
	i64 u0x5f36ccf5c6a57e24, ; 259: System.Xml.ReaderWriter.dll => 210
	i64 u0x5f4294b9b63cb842, ; 260: System.Data.Common => 139
	i64 u0x5f9a2d823f664957, ; 261: lib-el-Microsoft.Maui.Controls.resources.dll.so => 15
	i64 u0x5fac98e0b37a5b9d, ; 262: System.Runtime.CompilerServices.Unsafe.dll => 177
	i64 u0x609f4b7b63d802d4, ; 263: lib_Microsoft.Extensions.DependencyInjection.dll.so => 61
	i64 u0x60cd4e33d7e60134, ; 264: Xamarin.KotlinX.Coroutines.Core.Jvm => 121
	i64 u0x60f62d786afcf130, ; 265: System.Memory => 158
	i64 u0x618073e67851e2a7, ; 266: lib_K4os.Compression.LZ4.Streams.dll.so => 50
	i64 u0x61be8d1299194243, ; 267: Microsoft.Maui.Controls.Xaml => 76
	i64 u0x61d2cba29557038f, ; 268: de/Microsoft.Maui.Controls.resources => 14
	i64 u0x61d88f399afb2f45, ; 269: lib_System.Runtime.Loader.dll.so => 181
	i64 u0x622eef6f9e59068d, ; 270: System.Private.CoreLib => 215
	i64 u0x63f1f6883c1e23c2, ; 271: lib_System.Collections.Immutable.dll.so => 129
	i64 u0x6400f68068c1e9f1, ; 272: Xamarin.Google.Android.Material.dll => 119
	i64 u0x640e3b14dbd325c2, ; 273: System.Security.Cryptography.Algorithms.dll => 190
	i64 u0x65a51fb1cf95ad53, ; 274: ZstdSharp.dll => 123
	i64 u0x65ecac39144dd3cc, ; 275: Microsoft.Maui.Controls.dll => 75
	i64 u0x65ece51227bfa724, ; 276: lib_System.Runtime.Numerics.dll.so => 182
	i64 u0x6692e924eade1b29, ; 277: lib_System.Console.dll.so => 138
	i64 u0x66a4e5c6a3fb0bae, ; 278: lib_Xamarin.AndroidX.Lifecycle.ViewModel.Android.dll.so => 107
	i64 u0x66d13304ce1a3efa, ; 279: Xamarin.AndroidX.CursorAdapter => 101
	i64 u0x68558ec653afa616, ; 280: lib-da-Microsoft.Maui.Controls.resources.dll.so => 13
	i64 u0x6872ec7a2e36b1ac, ; 281: System.Drawing.Primitives.dll => 146
	i64 u0x68fbbbe2eb455198, ; 282: System.Formats.Asn1 => 148
	i64 u0x69063fc0ba8e6bdd, ; 283: he/Microsoft.Maui.Controls.resources.dll => 19
	i64 u0x6a4d7577b2317255, ; 284: System.Runtime.InteropServices.dll => 179
	i64 u0x6ace3b74b15ee4a4, ; 285: nb/Microsoft.Maui.Controls.resources => 28
	i64 u0x6b2e67033d722856, ; 286: MySql.EntityFrameworkCore => 82
	i64 u0x6d0a12b2adba20d8, ; 287: System.Security.Cryptography.ProtectedData.dll => 90
	i64 u0x6d12bfaa99c72b1f, ; 288: lib_Microsoft.Maui.Graphics.dll.so => 79
	i64 u0x6d70755158ca866e, ; 289: lib_System.ComponentModel.EventBasedAsync.dll.so => 134
	i64 u0x6d79993361e10ef2, ; 290: Microsoft.Extensions.Primitives => 66
	i64 u0x6d86d56b84c8eb71, ; 291: lib_Xamarin.AndroidX.CursorAdapter.dll.so => 101
	i64 u0x6d9bea6b3e895cf7, ; 292: Microsoft.Extensions.Primitives.dll => 66
	i64 u0x6e25a02c3833319a, ; 293: lib_Xamarin.AndroidX.Navigation.Fragment.dll.so => 111
	i64 u0x6fd2265da78b93a4, ; 294: lib_Microsoft.Maui.dll.so => 77
	i64 u0x6fdfc7de82c33008, ; 295: cs/Microsoft.Maui.Controls.resources => 12
	i64 u0x6ffc4967cc47ba57, ; 296: System.IO.FileSystem.Watcher.dll => 152
	i64 u0x70e99f48c05cb921, ; 297: tr/Microsoft.Maui.Controls.resources.dll => 38
	i64 u0x70fd3deda22442d2, ; 298: lib-nb-Microsoft.Maui.Controls.resources.dll.so => 28
	i64 u0x71a495ea3761dde8, ; 299: lib-it-Microsoft.Maui.Controls.resources.dll.so => 24
	i64 u0x71ad672adbe48f35, ; 300: System.ComponentModel.Primitives.dll => 135
	i64 u0x725f5a9e82a45c81, ; 301: System.Security.Cryptography.Encoding => 193
	i64 u0x72b1fb4109e08d7b, ; 302: lib-hr-Microsoft.Maui.Controls.resources.dll.so => 21
	i64 u0x73e4ce94e2eb6ffc, ; 303: lib_System.Memory.dll.so => 158
	i64 u0x755a91767330b3d4, ; 304: lib_Microsoft.Extensions.Configuration.dll.so => 59
	i64 u0x76012e7334db86e5, ; 305: lib_Xamarin.AndroidX.SavedState.dll.so => 115
	i64 u0x76ca07b878f44da0, ; 306: System.Runtime.Numerics.dll => 182
	i64 u0x777b4ed432c1e61e, ; 307: K4os.Compression.LZ4.Streams => 50
	i64 u0x77f8a4acc2fdc449, ; 308: System.Security.Cryptography.Cng.dll => 191
	i64 u0x780bc73597a503a9, ; 309: lib-ms-Microsoft.Maui.Controls.resources.dll.so => 27
	i64 u0x783606d1e53e7a1a, ; 310: th/Microsoft.Maui.Controls.resources.dll => 37
	i64 u0x7841c47b741b9f64, ; 311: System.Security.Permissions => 91
	i64 u0x7891b42e012cde6f, ; 312: Lotus Spor => 124
	i64 u0x78a45e51311409b6, ; 313: Xamarin.AndroidX.Fragment.dll => 104
	i64 u0x79eb916f2d11e1f0, ; 314: zh-Hans/Microsoft.Data.SqlClient.resources.dll => 8
	i64 u0x7adb8da2ac89b647, ; 315: fi/Microsoft.Maui.Controls.resources.dll => 17
	i64 u0x7b4927e421291c41, ; 316: Microsoft.IdentityModel.JsonWebTokens.dll => 70
	i64 u0x7bef86a4335c4870, ; 317: System.ComponentModel.TypeConverter => 136
	i64 u0x7c0820144cd34d6a, ; 318: sk/Microsoft.Maui.Controls.resources.dll => 35
	i64 u0x7c2a0bd1e0f988fc, ; 319: lib-de-Microsoft.Maui.Controls.resources.dll.so => 14
	i64 u0x7c41d387501568ba, ; 320: System.Net.WebClient.dll => 166
	i64 u0x7d649b75d580bb42, ; 321: ms/Microsoft.Maui.Controls.resources.dll => 27
	i64 u0x7d8ee2bdc8e3aad1, ; 322: System.Numerics.Vectors => 168
	i64 u0x7dfc3d6d9d8d7b70, ; 323: System.Collections => 132
	i64 u0x7e1f8f575a3599cb, ; 324: BouncyCastle.Cryptography.dll => 47
	i64 u0x7e2e564fa2f76c65, ; 325: lib_System.Diagnostics.Tracing.dll.so => 145
	i64 u0x7e302e110e1e1346, ; 326: lib_System.Security.Claims.dll.so => 189
	i64 u0x7e946809d6008ef2, ; 327: lib_System.ObjectModel.dll.so => 169
	i64 u0x7ecc13347c8fd849, ; 328: lib_System.ComponentModel.dll.so => 137
	i64 u0x7f00ddd9b9ca5a13, ; 329: Xamarin.AndroidX.ViewPager.dll => 117
	i64 u0x7f9351cd44b1273f, ; 330: Microsoft.Extensions.Configuration.Abstractions => 60
	i64 u0x7fae0ef4dc4770fe, ; 331: Microsoft.Identity.Client => 67
	i64 u0x7fbd557c99b3ce6f, ; 332: lib_Xamarin.AndroidX.Lifecycle.LiveData.Core.dll.so => 106
	i64 u0x812c069d5cdecc17, ; 333: System.dll => 213
	i64 u0x81ab745f6c0f5ce6, ; 334: zh-Hant/Microsoft.Maui.Controls.resources => 43
	i64 u0x82075fdf49c26af2, ; 335: ZstdSharp => 123
	i64 u0x8277f2be6b5ce05f, ; 336: Xamarin.AndroidX.AppCompat => 94
	i64 u0x828f06563b30bc50, ; 337: lib_Xamarin.AndroidX.CardView.dll.so => 97
	i64 u0x82df8f5532a10c59, ; 338: lib_System.Drawing.dll.so => 147
	i64 u0x82f6403342e12049, ; 339: uk/Microsoft.Maui.Controls.resources => 39
	i64 u0x83a7afd2c49adc86, ; 340: lib_Microsoft.IdentityModel.Abstractions.dll.so => 69
	i64 u0x83c14ba66c8e2b8c, ; 341: zh-Hans/Microsoft.Maui.Controls.resources => 42
	i64 u0x84ae73148a4557d2, ; 342: lib_System.IO.Pipes.dll.so => 154
	i64 u0x84b01102c12a9232, ; 343: System.Runtime.Serialization.Json.dll => 184
	i64 u0x84cd5cdec0f54bcc, ; 344: lib_Microsoft.EntityFrameworkCore.Relational.dll.so => 56
	i64 u0x8528b82bdbc15371, ; 345: ko/Microsoft.Data.SqlClient.resources => 5
	i64 u0x86a909228dc7657b, ; 346: lib-zh-Hant-Microsoft.Maui.Controls.resources.dll.so => 43
	i64 u0x86b3e00c36b84509, ; 347: Microsoft.Extensions.Configuration.dll => 59
	i64 u0x86b62cb077ec4fd7, ; 348: System.Runtime.Serialization.Xml => 186
	i64 u0x87c4b8a492b176ad, ; 349: Microsoft.EntityFrameworkCore.Abstractions => 55
	i64 u0x87c69b87d9283884, ; 350: lib_System.Threading.Thread.dll.so => 204
	i64 u0x87f6569b25707834, ; 351: System.IO.Compression.Brotli.dll => 149
	i64 u0x8842b3a5d2d3fb36, ; 352: Microsoft.Maui.Essentials => 78
	i64 u0x88bda98e0cffb7a9, ; 353: lib_Xamarin.KotlinX.Coroutines.Core.Jvm.dll.so => 121
	i64 u0x8930322c7bd8f768, ; 354: netstandard => 214
	i64 u0x897a606c9e39c75f, ; 355: lib_System.ComponentModel.Primitives.dll.so => 135
	i64 u0x89c5188089ec2cd5, ; 356: lib_System.Runtime.InteropServices.RuntimeInformation.dll.so => 178
	i64 u0x8a399a706fcbce4b, ; 357: Microsoft.Extensions.Caching.Abstractions => 57
	i64 u0x8ad229ea26432ee2, ; 358: Xamarin.AndroidX.Loader => 109
	i64 u0x8b4ff5d0fdd5faa1, ; 359: lib_System.Diagnostics.DiagnosticSource.dll.so => 140
	i64 u0x8b541d476eb3774c, ; 360: System.Security.Principal.Windows => 197
	i64 u0x8b8d01333a96d0b5, ; 361: System.Diagnostics.Process.dll => 141
	i64 u0x8b9ceca7acae3451, ; 362: lib-he-Microsoft.Maui.Controls.resources.dll.so => 19
	i64 u0x8c1bafb2ed25af5b, ; 363: K4os.Compression.LZ4.Streams.dll => 50
	i64 u0x8c53ae18581b14f0, ; 364: Azure.Core => 45
	i64 u0x8cdfdb4ce85fb925, ; 365: lib_System.Security.Principal.Windows.dll.so => 197
	i64 u0x8d0f420977c2c1c7, ; 366: Xamarin.AndroidX.CursorAdapter.dll => 101
	i64 u0x8d7b8ab4b3310ead, ; 367: System.Threading => 206
	i64 u0x8da188285aadfe8e, ; 368: System.Collections.Concurrent => 128
	i64 u0x8e937db395a74375, ; 369: lib_Microsoft.Identity.Client.dll.so => 67
	i64 u0x8ed3cdd722b4d782, ; 370: System.Diagnostics.EventLog => 86
	i64 u0x8ed807bfe9858dfc, ; 371: Xamarin.AndroidX.Navigation.Common => 110
	i64 u0x8ee08b8194a30f48, ; 372: lib-hi-Microsoft.Maui.Controls.resources.dll.so => 20
	i64 u0x8ef7601039857a44, ; 373: lib-ro-Microsoft.Maui.Controls.resources.dll.so => 33
	i64 u0x8f32c6f611f6ffab, ; 374: pt/Microsoft.Maui.Controls.resources.dll => 32
	i64 u0x8f8829d21c8985a4, ; 375: lib-pt-BR-Microsoft.Maui.Controls.resources.dll.so => 31
	i64 u0x90263f8448b8f572, ; 376: lib_System.Diagnostics.TraceSource.dll.so => 144
	i64 u0x903101b46fb73a04, ; 377: _Microsoft.Android.Resource.Designer => 44
	i64 u0x90393bd4865292f3, ; 378: lib_System.IO.Compression.dll.so => 150
	i64 u0x905e2b8e7ae91ae6, ; 379: System.Threading.Tasks.Extensions.dll => 203
	i64 u0x90634f86c5ebe2b5, ; 380: Xamarin.AndroidX.Lifecycle.ViewModel.Android => 107
	i64 u0x907b636704ad79ef, ; 381: lib_Microsoft.Maui.Controls.Xaml.dll.so => 76
	i64 u0x91418dc638b29e68, ; 382: lib_Xamarin.AndroidX.CustomView.dll.so => 102
	i64 u0x9157bd523cd7ed36, ; 383: lib_System.Text.Json.dll.so => 92
	i64 u0x91a74f07b30d37e2, ; 384: System.Linq.dll => 157
	i64 u0x91fa41a87223399f, ; 385: ca/Microsoft.Maui.Controls.resources.dll => 11
	i64 u0x93489853b6098685, ; 386: es/Microsoft.Data.SqlClient.resources.dll => 1
	i64 u0x93cfa73ab28d6e35, ; 387: ms/Microsoft.Maui.Controls.resources => 27
	i64 u0x944077d8ca3c6580, ; 388: System.IO.Compression.dll => 150
	i64 u0x948d746a7702861f, ; 389: Microsoft.IdentityModel.Logging.dll => 71
	i64 u0x9502fd818eed2359, ; 390: lib_Microsoft.IdentityModel.Protocols.OpenIdConnect.dll.so => 73
	i64 u0x9564283c37ed59a9, ; 391: lib_Microsoft.IdentityModel.Logging.dll.so => 71
	i64 u0x967fc325e09bfa8c, ; 392: es/Microsoft.Maui.Controls.resources => 16
	i64 u0x96e49b31fe33d427, ; 393: Microsoft.Identity.Client.Extensions.Msal => 68
	i64 u0x9732d8dbddea3d9a, ; 394: id/Microsoft.Maui.Controls.resources => 23
	i64 u0x978be80e5210d31b, ; 395: Microsoft.Maui.Graphics.dll => 79
	i64 u0x97b8c771ea3e4220, ; 396: System.ComponentModel.dll => 137
	i64 u0x97e144c9d3c6976e, ; 397: System.Collections.Concurrent.dll => 128
	i64 u0x991d510397f92d9d, ; 398: System.Linq.Expressions => 155
	i64 u0x99868af5d93ecaeb, ; 399: lib_K4os.Hash.xxHash.dll.so => 51
	i64 u0x99a00ca5270c6878, ; 400: Xamarin.AndroidX.Navigation.Runtime => 112
	i64 u0x99cdc6d1f2d3a72f, ; 401: ko/Microsoft.Maui.Controls.resources.dll => 26
	i64 u0x9a0cc42c6f36dfc9, ; 402: lib_Microsoft.IdentityModel.Protocols.dll.so => 72
	i64 u0x9b211a749105beac, ; 403: System.Transactions.Local => 207
	i64 u0x9c244ac7cda32d26, ; 404: System.Security.Cryptography.X509Certificates.dll => 195
	i64 u0x9d5dbcf5a48583fe, ; 405: lib_Xamarin.AndroidX.Activity.dll.so => 93
	i64 u0x9d74dee1a7725f34, ; 406: Microsoft.Extensions.Configuration.Abstractions.dll => 60
	i64 u0x9e4534b6adaf6e84, ; 407: nl/Microsoft.Maui.Controls.resources => 29
	i64 u0x9eaf1efdf6f7267e, ; 408: Xamarin.AndroidX.Navigation.Common.dll => 110
	i64 u0x9ef542cf1f78c506, ; 409: Xamarin.AndroidX.Lifecycle.LiveData.Core => 106
	i64 u0x9ffbb6b1434ad2df, ; 410: Microsoft.Identity.Client.dll => 67
	i64 u0xa0d8259f4cc284ec, ; 411: lib_System.Security.Cryptography.dll.so => 196
	i64 u0xa1440773ee9d341e, ; 412: Xamarin.Google.Android.Material => 119
	i64 u0xa1b9d7c27f47219f, ; 413: Xamarin.AndroidX.Navigation.UI.dll => 113
	i64 u0xa2572680829d2c7c, ; 414: System.IO.Pipelines.dll => 153
	i64 u0xa46aa1eaa214539b, ; 415: ko/Microsoft.Maui.Controls.resources => 26
	i64 u0xa4edc8f2ceae241a, ; 416: System.Data.Common.dll => 139
	i64 u0xa5494f40f128ce6a, ; 417: System.Runtime.Serialization.Formatters.dll => 183
	i64 u0xa5b7152421ed6d98, ; 418: lib_System.IO.FileSystem.Watcher.dll.so => 152
	i64 u0xa5ce5c755bde8cb8, ; 419: lib_System.Security.Cryptography.Csp.dll.so => 192
	i64 u0xa5e599d1e0524750, ; 420: System.Numerics.Vectors.dll => 168
	i64 u0xa5f1ba49b85dd355, ; 421: System.Security.Cryptography.dll => 196
	i64 u0xa61975a5a37873ea, ; 422: lib_System.Xml.XmlSerializer.dll.so => 212
	i64 u0xa64476a892d76457, ; 423: lib_MySql.Data.dll.so => 81
	i64 u0xa67dbee13e1df9ca, ; 424: Xamarin.AndroidX.SavedState.dll => 115
	i64 u0xa68a420042bb9b1f, ; 425: Xamarin.AndroidX.DrawerLayout.dll => 103
	i64 u0xa71fe7d6f6f93efd, ; 426: Microsoft.Data.SqlClient => 53
	i64 u0xa763fbb98df8d9fb, ; 427: lib_Microsoft.Win32.Primitives.dll.so => 126
	i64 u0xa78ce3745383236a, ; 428: Xamarin.AndroidX.Lifecycle.Common.Jvm => 105
	i64 u0xa7c31b56b4dc7b33, ; 429: hu/Microsoft.Maui.Controls.resources => 22
	i64 u0xa8e6320dd07580ef, ; 430: lib_Microsoft.IdentityModel.JsonWebTokens.dll.so => 70
	i64 u0xaa2219c8e3449ff5, ; 431: Microsoft.Extensions.Logging.Abstractions => 64
	i64 u0xaa443ac34067eeef, ; 432: System.Private.Xml.dll => 173
	i64 u0xaa52de307ef5d1dd, ; 433: System.Net.Http => 159
	i64 u0xaa9a7b0214a5cc5c, ; 434: System.Diagnostics.StackTrace.dll => 142
	i64 u0xaaaf86367285a918, ; 435: Microsoft.Extensions.DependencyInjection.Abstractions.dll => 62
	i64 u0xaaf84bb3f052a265, ; 436: el/Microsoft.Maui.Controls.resources => 15
	i64 u0xab9c1b2687d86b0b, ; 437: lib_System.Linq.Expressions.dll.so => 155
	i64 u0xac2af3fa195a15ce, ; 438: System.Runtime.Numerics => 182
	i64 u0xac5376a2a538dc10, ; 439: Xamarin.AndroidX.Lifecycle.LiveData.Core.dll => 106
	i64 u0xac65e40f62b6b90e, ; 440: Google.Protobuf => 48
	i64 u0xac79c7e46047ad98, ; 441: System.Security.Principal.Windows.dll => 197
	i64 u0xac98d31068e24591, ; 442: System.Xml.XDocument => 211
	i64 u0xacd46e002c3ccb97, ; 443: ro/Microsoft.Maui.Controls.resources => 33
	i64 u0xacf42eea7ef9cd12, ; 444: System.Threading.Channels => 202
	i64 u0xad89c07347f1bad6, ; 445: nl/Microsoft.Maui.Controls.resources.dll => 29
	i64 u0xadbb53caf78a79d2, ; 446: System.Web.HttpUtility => 208
	i64 u0xadc90ab061a9e6e4, ; 447: System.ComponentModel.TypeConverter.dll => 136
	i64 u0xadf511667bef3595, ; 448: System.Net.Security => 164
	i64 u0xae0aaa94fdcfce0f, ; 449: System.ComponentModel.EventBasedAsync.dll => 134
	i64 u0xae282bcd03739de7, ; 450: Java.Interop => 216
	i64 u0xae53579c90db1107, ; 451: System.ObjectModel.dll => 169
	i64 u0xafe29f45095518e7, ; 452: lib_Xamarin.AndroidX.Lifecycle.ViewModelSavedState.dll.so => 108
	i64 u0xb05cc42cd94c6d9d, ; 453: lib-sv-Microsoft.Maui.Controls.resources.dll.so => 36
	i64 u0xb0bb43dc52ea59f9, ; 454: System.Diagnostics.Tracing.dll => 145
	i64 u0xb1dd05401aa8ee63, ; 455: System.Security.AccessControl => 188
	i64 u0xb220631954820169, ; 456: System.Text.RegularExpressions => 201
	i64 u0xb2376e1dbf8b4ed7, ; 457: System.Security.Cryptography.Csp => 192
	i64 u0xb2a3f67f3bf29fce, ; 458: da/Microsoft.Maui.Controls.resources => 13
	i64 u0xb398860d6ed7ba2f, ; 459: System.Security.Cryptography.ProtectedData => 90
	i64 u0xb3f0a0fcda8d3ebc, ; 460: Xamarin.AndroidX.CardView => 97
	i64 u0xb46be1aa6d4fff93, ; 461: hi/Microsoft.Maui.Controls.resources => 20
	i64 u0xb477491be13109d8, ; 462: ar/Microsoft.Maui.Controls.resources => 10
	i64 u0xb4bd7015ecee9d86, ; 463: System.IO.Pipelines => 153
	i64 u0xb4c53d9749c5f226, ; 464: lib_System.IO.FileSystem.AccessControl.dll.so => 151
	i64 u0xb5c7fcdafbc67ee4, ; 465: Microsoft.Extensions.Logging.Abstractions.dll => 64
	i64 u0xb5ea31d5244c6626, ; 466: System.Threading.ThreadPool.dll => 205
	i64 u0xb7212c4683a94afe, ; 467: System.Drawing.Primitives => 146
	i64 u0xb7b7753d1f319409, ; 468: sv/Microsoft.Maui.Controls.resources => 36
	i64 u0xb81a2c6e0aee50fe, ; 469: lib_System.Private.CoreLib.dll.so => 215
	i64 u0xb9185c33a1643eed, ; 470: Microsoft.CSharp.dll => 125
	i64 u0xb9f64d3b230def68, ; 471: lib-pt-Microsoft.Maui.Controls.resources.dll.so => 32
	i64 u0xb9fc3c8a556e3691, ; 472: ja/Microsoft.Maui.Controls.resources => 25
	i64 u0xba4670aa94a2b3c6, ; 473: lib_System.Xml.XDocument.dll.so => 211
	i64 u0xba48785529705af9, ; 474: System.Collections.dll => 132
	i64 u0xbadbc0a44214b54e, ; 475: K4os.Compression.LZ4 => 49
	i64 u0xbb65706fde942ce3, ; 476: System.Net.Sockets => 165
	i64 u0xbb8c8d165ef11460, ; 477: lib_Microsoft.Identity.Client.Extensions.Msal.dll.so => 68
	i64 u0xbbd180354b67271a, ; 478: System.Runtime.Serialization.Formatters => 183
	i64 u0xbcd22b365b764643, ; 479: lib-zh-Hans-Microsoft.Data.SqlClient.resources.dll.so => 8
	i64 u0xbcfa7c134d2089f3, ; 480: System.Runtime.Caching => 89
	i64 u0xbd0aaf9dbfcc3376, ; 481: fr/Microsoft.Data.SqlClient.resources.dll => 2
	i64 u0xbd0e2c0d55246576, ; 482: System.Net.Http.dll => 159
	i64 u0xbd3c2d7a8325e11b, ; 483: lib-fr-Microsoft.Data.SqlClient.resources.dll.so => 2
	i64 u0xbd437a2cdb333d0d, ; 484: Xamarin.AndroidX.ViewPager2 => 118
	i64 u0xbd4aef17dbfb0390, ; 485: ru/Microsoft.Data.SqlClient.resources => 7
	i64 u0xbd5d0b88d3d647a5, ; 486: lib_Xamarin.AndroidX.Browser.dll.so => 96
	i64 u0xbd877b14d0b56392, ; 487: System.Runtime.Intrinsics.dll => 180
	i64 u0xbe65a49036345cf4, ; 488: lib_System.Buffers.dll.so => 127
	i64 u0xbee38d4a88835966, ; 489: Xamarin.AndroidX.AppCompat.AppCompatResources => 95
	i64 u0xc040a4ab55817f58, ; 490: ar/Microsoft.Maui.Controls.resources.dll => 10
	i64 u0xc0d928351ab5ca77, ; 491: System.Console.dll => 138
	i64 u0xc0f5a221a9383aea, ; 492: System.Runtime.Intrinsics => 180
	i64 u0xc12b8b3afa48329c, ; 493: lib_System.Linq.dll.so => 157
	i64 u0xc1c2cb7af77b8858, ; 494: Microsoft.EntityFrameworkCore => 54
	i64 u0xc1ff9ae3cdb6e1e6, ; 495: Xamarin.AndroidX.Activity.dll => 93
	i64 u0xc2260e1da1054ac1, ; 496: lib_BouncyCastle.Cryptography.dll.so => 47
	i64 u0xc26c064effb1dea9, ; 497: System.Buffers.dll => 127
	i64 u0xc278de356ad8a9e3, ; 498: Microsoft.IdentityModel.Logging => 71
	i64 u0xc28c50f32f81cc73, ; 499: ja/Microsoft.Maui.Controls.resources.dll => 25
	i64 u0xc2a3bca55b573141, ; 500: System.IO.FileSystem.Watcher => 152
	i64 u0xc2bcfec99f69365e, ; 501: Xamarin.AndroidX.ViewPager2.dll => 118
	i64 u0xc30b52815b58ac2c, ; 502: lib_System.Runtime.Serialization.Xml.dll.so => 186
	i64 u0xc463e077917aa21d, ; 503: System.Runtime.Serialization.Json => 184
	i64 u0xc472ce300460ccb6, ; 504: Microsoft.EntityFrameworkCore.dll => 54
	i64 u0xc4d3858ed4d08512, ; 505: Xamarin.AndroidX.Lifecycle.ViewModelSavedState.dll => 108
	i64 u0xc4d69851fe06342f, ; 506: lib_Microsoft.Extensions.Caching.Memory.dll.so => 58
	i64 u0xc50fded0ded1418c, ; 507: lib_System.ComponentModel.TypeConverter.dll.so => 136
	i64 u0xc519125d6bc8fb11, ; 508: lib_System.Net.Requests.dll.so => 163
	i64 u0xc5293b19e4dc230e, ; 509: Xamarin.AndroidX.Navigation.Fragment => 111
	i64 u0xc5325b2fcb37446f, ; 510: lib_System.Private.Xml.dll.so => 173
	i64 u0xc583d8477b5d3bac, ; 511: zh-Hant/Microsoft.Data.SqlClient.resources.dll => 9
	i64 u0xc5a0f4b95a699af7, ; 512: lib_System.Private.Uri.dll.so => 171
	i64 u0xc5cdcd5b6277579e, ; 513: lib_System.Security.Cryptography.Algorithms.dll.so => 190
	i64 u0xc6a4665a88c57225, ; 514: lib_ZstdSharp.dll.so => 123
	i64 u0xc7c01e7d7c93a110, ; 515: System.Text.Encoding.Extensions.dll => 199
	i64 u0xc7ce851898a4548e, ; 516: lib_System.Web.HttpUtility.dll.so => 208
	i64 u0xc858a28d9ee5a6c5, ; 517: lib_System.Collections.Specialized.dll.so => 131
	i64 u0xc9c62c8f354ac568, ; 518: lib_System.Diagnostics.TextWriterTraceListener.dll.so => 143
	i64 u0xca32340d8d54dcd5, ; 519: Microsoft.Extensions.Caching.Memory.dll => 58
	i64 u0xca3a723e7342c5b6, ; 520: lib-tr-Microsoft.Maui.Controls.resources.dll.so => 38
	i64 u0xcab3493c70141c2d, ; 521: pl/Microsoft.Maui.Controls.resources => 30
	i64 u0xcacfddc9f7c6de76, ; 522: ro/Microsoft.Maui.Controls.resources.dll => 33
	i64 u0xcb45618372c47127, ; 523: Microsoft.EntityFrameworkCore.Relational => 56
	i64 u0xcbd4fdd9cef4a294, ; 524: lib__Microsoft.Android.Resource.Designer.dll.so => 44
	i64 u0xcc182c3afdc374d6, ; 525: Microsoft.Bcl.AsyncInterfaces => 52
	i64 u0xcc2876b32ef2794c, ; 526: lib_System.Text.RegularExpressions.dll.so => 201
	i64 u0xcc5c3bb714c4561e, ; 527: Xamarin.KotlinX.Coroutines.Core.Jvm.dll => 121
	i64 u0xcc76886e09b88260, ; 528: Xamarin.KotlinX.Serialization.Core.Jvm.dll => 122
	i64 u0xccf25c4b634ccd3a, ; 529: zh-Hans/Microsoft.Maui.Controls.resources.dll => 42
	i64 u0xcd10a42808629144, ; 530: System.Net.Requests => 163
	i64 u0xcd3586b93136841e, ; 531: lib_System.Runtime.Caching.dll.so => 89
	i64 u0xcdd0c48b6937b21c, ; 532: Xamarin.AndroidX.SwipeRefreshLayout => 116
	i64 u0xceb28d385f84f441, ; 533: Azure.Core.dll => 45
	i64 u0xcf140ed700bc8e66, ; 534: Microsoft.SqlServer.Server.dll => 80
	i64 u0xcf23d8093f3ceadf, ; 535: System.Diagnostics.DiagnosticSource.dll => 140
	i64 u0xcf8fc898f98b0d34, ; 536: System.Private.Xml.Linq => 172
	i64 u0xd063299fcfc0c93f, ; 537: lib_System.Runtime.Serialization.Json.dll.so => 184
	i64 u0xd0de8a113e976700, ; 538: System.Diagnostics.TextWriterTraceListener => 143
	i64 u0xd1194e1d8a8de83c, ; 539: lib_Xamarin.AndroidX.Lifecycle.Common.Jvm.dll.so => 105
	i64 u0xd22a0c4630f2fe66, ; 540: lib_System.Security.Cryptography.ProtectedData.dll.so => 90
	i64 u0xd2dffb59201927bd, ; 541: de/Microsoft.Data.SqlClient.resources.dll => 0
	i64 u0xd333d0af9e423810, ; 542: System.Runtime.InteropServices => 179
	i64 u0xd33a415cb4278969, ; 543: System.Security.Cryptography.Encoding.dll => 193
	i64 u0xd3426d966bb704f5, ; 544: Xamarin.AndroidX.AppCompat.AppCompatResources.dll => 95
	i64 u0xd3651b6fc3125825, ; 545: System.Private.Uri.dll => 171
	i64 u0xd373685349b1fe8b, ; 546: Microsoft.Extensions.Logging.dll => 63
	i64 u0xd3801faafafb7698, ; 547: System.Private.DataContractSerialization.dll => 170
	i64 u0xd3e4c8d6a2d5d470, ; 548: it/Microsoft.Maui.Controls.resources => 24
	i64 u0xd42655883bb8c19f, ; 549: Microsoft.EntityFrameworkCore.Abstractions.dll => 55
	i64 u0xd4645626dffec99d, ; 550: lib_Microsoft.Extensions.DependencyInjection.Abstractions.dll.so => 62
	i64 u0xd5507e11a2b2839f, ; 551: Xamarin.AndroidX.Lifecycle.ViewModelSavedState => 108
	i64 u0xd5858610826f1c08, ; 552: lib-ru-Microsoft.Data.SqlClient.resources.dll.so => 7
	i64 u0xd6694f8359737e4e, ; 553: Xamarin.AndroidX.SavedState => 115
	i64 u0xd6d21782156bc35b, ; 554: Xamarin.AndroidX.SwipeRefreshLayout.dll => 116
	i64 u0xd72329819cbbbc44, ; 555: lib_Microsoft.Extensions.Configuration.Abstractions.dll.so => 60
	i64 u0xd72c760af136e863, ; 556: System.Xml.XmlSerializer.dll => 212
	i64 u0xd7b3764ada9d341d, ; 557: lib_Microsoft.Extensions.Logging.Abstractions.dll.so => 64
	i64 u0xd9e245a1762ddad5, ; 558: BouncyCastle.Cryptography => 47
	i64 u0xda1dfa4c534a9251, ; 559: Microsoft.Extensions.DependencyInjection => 61
	i64 u0xdad05a11827959a3, ; 560: System.Collections.NonGeneric.dll => 130
	i64 u0xdb5383ab5865c007, ; 561: lib-vi-Microsoft.Maui.Controls.resources.dll.so => 40
	i64 u0xdb58816721c02a59, ; 562: lib_System.Reflection.Emit.ILGeneration.dll.so => 174
	i64 u0xdbeda89f832aa805, ; 563: vi/Microsoft.Maui.Controls.resources.dll => 40
	i64 u0xdbf2a779fbc3ac31, ; 564: System.Transactions.Local.dll => 207
	i64 u0xdbf9607a441b4505, ; 565: System.Linq => 157
	i64 u0xdc75032002d1a212, ; 566: lib_System.Transactions.Local.dll.so => 207
	i64 u0xdca8be7403f92d4f, ; 567: lib_System.Linq.Queryable.dll.so => 156
	i64 u0xdce2c53525640bf3, ; 568: Microsoft.Extensions.Logging => 63
	i64 u0xdd2b722d78ef5f43, ; 569: System.Runtime.dll => 187
	i64 u0xdd67031857c72f96, ; 570: lib_System.Text.Encodings.Web.dll.so => 200
	i64 u0xdde30e6b77aa6f6c, ; 571: lib-zh-Hans-Microsoft.Maui.Controls.resources.dll.so => 42
	i64 u0xde110ae80fa7c2e2, ; 572: System.Xml.XDocument.dll => 211
	i64 u0xde572c2b2fb32f93, ; 573: lib_System.Threading.Tasks.Extensions.dll.so => 203
	i64 u0xde8769ebda7d8647, ; 574: hr/Microsoft.Maui.Controls.resources.dll => 21
	i64 u0xdf35b6d818902893, ; 575: K4os.Hash.xxHash.dll => 51
	i64 u0xe0142572c095a480, ; 576: Xamarin.AndroidX.AppCompat.dll => 94
	i64 u0xe02f89350ec78051, ; 577: Xamarin.AndroidX.CoordinatorLayout.dll => 99
	i64 u0xe0ea30f1ac5b7731, ; 578: ko/Microsoft.Data.SqlClient.resources.dll => 5
	i64 u0xe0ee2e61123c1478, ; 579: lib-es-Microsoft.Data.SqlClient.resources.dll.so => 1
	i64 u0xe10b760bb1462e7a, ; 580: lib_System.Security.Cryptography.Primitives.dll.so => 194
	i64 u0xe12265280d0b036d, ; 581: fr/Microsoft.Data.SqlClient.resources => 2
	i64 u0xe192a588d4410686, ; 582: lib_System.IO.Pipelines.dll.so => 153
	i64 u0xe1a08bd3fa539e0d, ; 583: System.Runtime.Loader => 181
	i64 u0xe1b52f9f816c70ef, ; 584: System.Private.Xml.Linq.dll => 172
	i64 u0xe1ecfdb7fff86067, ; 585: System.Net.Security.dll => 164
	i64 u0xe22fa4c9c645db62, ; 586: System.Diagnostics.TextWriterTraceListener.dll => 143
	i64 u0xe2420585aeceb728, ; 587: System.Net.Requests.dll => 163
	i64 u0xe29b73bc11392966, ; 588: lib-id-Microsoft.Maui.Controls.resources.dll.so => 23
	i64 u0xe2e426c7714fa0bc, ; 589: Microsoft.Win32.Primitives.dll => 126
	i64 u0xe3811d68d4fe8463, ; 590: pt-BR/Microsoft.Maui.Controls.resources.dll => 31
	i64 u0xe3b7cbae5ad66c75, ; 591: lib_System.Security.Cryptography.Encoding.dll.so => 193
	i64 u0xe494f7ced4ecd10a, ; 592: hu/Microsoft.Maui.Controls.resources.dll => 22
	i64 u0xe4a9b1e40d1e8917, ; 593: lib-fi-Microsoft.Maui.Controls.resources.dll.so => 17
	i64 u0xe4f74a0b5bf9703f, ; 594: System.Runtime.Serialization.Primitives => 185
	i64 u0xe5434e8a119ceb69, ; 595: lib_Mono.Android.dll.so => 218
	i64 u0xe57d22ca4aeb4900, ; 596: System.Configuration.ConfigurationManager => 84
	i64 u0xe79d45aa815dab7f, ; 597: System.Runtime.Caching.dll => 89
	i64 u0xe7e03cc18dcdeb49, ; 598: lib_System.Diagnostics.StackTrace.dll.so => 142
	i64 u0xe89a2a9ef110899b, ; 599: System.Drawing.dll => 147
	i64 u0xe95d3096c1827a3f, ; 600: lib_MySql.EntityFrameworkCore.dll.so => 82
	i64 u0xed6ef763c6fb395f, ; 601: System.Diagnostics.EventLog.dll => 86
	i64 u0xedc4817167106c23, ; 602: System.Net.Sockets.dll => 165
	i64 u0xedc632067fb20ff3, ; 603: System.Memory.dll => 158
	i64 u0xedc8e4ca71a02a8b, ; 604: Xamarin.AndroidX.Navigation.Runtime.dll => 112
	i64 u0xee81f5b3f1c4f83b, ; 605: System.Threading.ThreadPool => 205
	i64 u0xeeb7ebb80150501b, ; 606: lib_Xamarin.AndroidX.Collection.Jvm.dll.so => 98
	i64 u0xeefc635595ef57f0, ; 607: System.Security.Cryptography.Cng => 191
	i64 u0xef03b1b5a04e9709, ; 608: System.Text.Encoding.CodePages.dll => 198
	i64 u0xef72742e1bcca27a, ; 609: Microsoft.Maui.Essentials.dll => 78
	i64 u0xefd0396433f04886, ; 610: pt-BR/Microsoft.Data.SqlClient.resources => 6
	i64 u0xefec0b7fdc57ec42, ; 611: Xamarin.AndroidX.Activity => 93
	i64 u0xf00c29406ea45e19, ; 612: es/Microsoft.Maui.Controls.resources.dll => 16
	i64 u0xf09e47b6ae914f6e, ; 613: System.Net.NameResolution => 160
	i64 u0xf0de2537ee19c6ca, ; 614: lib_System.Net.WebHeaderCollection.dll.so => 167
	i64 u0xf11b621fc87b983f, ; 615: Microsoft.Maui.Controls.Xaml.dll => 76
	i64 u0xf1c4b4005493d871, ; 616: System.Formats.Asn1.dll => 148
	i64 u0xf238bd79489d3a96, ; 617: lib-nl-Microsoft.Maui.Controls.resources.dll.so => 29
	i64 u0xf37221fda4ef8830, ; 618: lib_Xamarin.Google.Android.Material.dll.so => 119
	i64 u0xf3ddfe05336abf29, ; 619: System => 213
	i64 u0xf408654b2a135055, ; 620: System.Reflection.Emit.ILGeneration.dll => 174
	i64 u0xf4103170a1de5bd0, ; 621: System.Linq.Queryable.dll => 156
	i64 u0xf4c1dd70a5496a17, ; 622: System.IO.Compression => 150
	i64 u0xf5e59d7ac34b50aa, ; 623: Microsoft.IdentityModel.Protocols.dll => 72
	i64 u0xf5fc7602fe27b333, ; 624: System.Net.WebHeaderCollection => 167
	i64 u0xf6077741019d7428, ; 625: Xamarin.AndroidX.CoordinatorLayout => 99
	i64 u0xf61ade9836ad4692, ; 626: Microsoft.IdentityModel.Tokens.dll => 74
	i64 u0xf6c0e7d55a7a4e4f, ; 627: Microsoft.IdentityModel.JsonWebTokens => 70
	i64 u0xf77b20923f07c667, ; 628: de/Microsoft.Maui.Controls.resources.dll => 14
	i64 u0xf7be8a85d06b4b64, ; 629: ru/Microsoft.Data.SqlClient.resources.dll => 7
	i64 u0xf7e2cac4c45067b3, ; 630: lib_System.Numerics.Vectors.dll.so => 168
	i64 u0xf7e74930e0e3d214, ; 631: zh-HK/Microsoft.Maui.Controls.resources.dll => 41
	i64 u0xf83775f330791063, ; 632: ja/Microsoft.Data.SqlClient.resources.dll => 4
	i64 u0xf84773b5c81e3cef, ; 633: lib-uk-Microsoft.Maui.Controls.resources.dll.so => 39
	i64 u0xf8aac5ea82de1348, ; 634: System.Linq.Queryable => 156
	i64 u0xf8b77539b362d3ba, ; 635: lib_System.Reflection.Primitives.dll.so => 176
	i64 u0xf8cd217ba1bbfdc8, ; 636: lib-zh-Hant-Microsoft.Data.SqlClient.resources.dll.so => 9
	i64 u0xf8e045dc345b2ea3, ; 637: lib_Xamarin.AndroidX.RecyclerView.dll.so => 114
	i64 u0xf915dc29808193a1, ; 638: System.Web.HttpUtility.dll => 208
	i64 u0xf96c777a2a0686f4, ; 639: hi/Microsoft.Maui.Controls.resources.dll => 20
	i64 u0xf9be54c8bcf8ff3b, ; 640: System.Security.AccessControl.dll => 188
	i64 u0xf9eec5bb3a6aedc6, ; 641: Microsoft.Extensions.Options => 65
	i64 u0xfa2fdb27e8a2c8e8, ; 642: System.ComponentModel.EventBasedAsync => 134
	i64 u0xfa3f278f288b0e84, ; 643: lib_System.Net.Security.dll.so => 164
	i64 u0xfa5ed7226d978949, ; 644: lib-ar-Microsoft.Maui.Controls.resources.dll.so => 10
	i64 u0xfa645d91e9fc4cba, ; 645: System.Threading.Thread => 204
	i64 u0xfbad3e4ce4b98145, ; 646: System.Security.Cryptography.X509Certificates => 195
	i64 u0xfbf0a31c9fc34bc4, ; 647: lib_System.Net.Http.dll.so => 159
	i64 u0xfc6b7527cc280b3f, ; 648: lib_System.Runtime.Serialization.Formatters.dll.so => 183
	i64 u0xfc719aec26adf9d9, ; 649: Xamarin.AndroidX.Navigation.Fragment.dll => 111
	i64 u0xfcd5b90cf101e36b, ; 650: System.Data.SqlClient.dll => 85
	i64 u0xfd22f00870e40ae0, ; 651: lib_Xamarin.AndroidX.DrawerLayout.dll.so => 103
	i64 u0xfd49b3c1a76e2748, ; 652: System.Runtime.InteropServices.RuntimeInformation => 178
	i64 u0xfd536c702f64dc47, ; 653: System.Text.Encoding.Extensions => 199
	i64 u0xfd583f7657b6a1cb, ; 654: Xamarin.AndroidX.Fragment => 104
	i64 u0xfeae9952cf03b8cb, ; 655: tr/Microsoft.Maui.Controls.resources => 38
	i64 u0xfff40914e0b38d3d ; 656: Azure.Identity.dll => 46
], align 8

@assembly_image_cache_indices = dso_local local_unnamed_addr constant [657 x i32] [
	i32 0, i32 116, i32 133, i32 112, i32 5, i32 58, i32 217, i32 94,
	i32 127, i32 170, i32 34, i32 12, i32 40, i32 69, i32 162, i32 114,
	i32 132, i32 77, i32 177, i32 9, i32 41, i32 209, i32 98, i32 34,
	i32 130, i32 3, i32 45, i32 176, i32 103, i32 133, i32 65, i32 130,
	i32 88, i32 196, i32 57, i32 202, i32 53, i32 35, i32 122, i32 117,
	i32 31, i32 218, i32 78, i32 52, i32 160, i32 4, i32 52, i32 102,
	i32 1, i32 149, i32 194, i32 175, i32 49, i32 114, i32 73, i32 96,
	i32 18, i32 216, i32 19, i32 73, i32 62, i32 126, i32 86, i32 0,
	i32 154, i32 190, i32 214, i32 188, i32 22, i32 200, i32 122, i32 166,
	i32 28, i32 189, i32 46, i32 128, i32 213, i32 37, i32 217, i32 151,
	i32 48, i32 142, i32 113, i32 26, i32 65, i32 212, i32 166, i32 149,
	i32 91, i32 141, i32 187, i32 83, i32 174, i32 37, i32 154, i32 49,
	i32 204, i32 54, i32 124, i32 138, i32 100, i32 185, i32 18, i32 120,
	i32 66, i32 87, i32 23, i32 21, i32 216, i32 162, i32 124, i32 39,
	i32 161, i32 144, i32 17, i32 201, i32 87, i32 148, i32 43, i32 30,
	i32 198, i32 175, i32 172, i32 6, i32 206, i32 36, i32 92, i32 15,
	i32 141, i32 210, i32 80, i32 145, i32 57, i32 104, i32 69, i32 44,
	i32 82, i32 97, i32 146, i32 18, i32 210, i32 129, i32 16, i32 165,
	i32 80, i32 77, i32 12, i32 75, i32 133, i32 118, i32 59, i32 180,
	i32 175, i32 176, i32 129, i32 177, i32 102, i32 160, i32 72, i32 84,
	i32 117, i32 11, i32 194, i32 81, i32 199, i32 74, i32 195, i32 53,
	i32 120, i32 96, i32 85, i32 205, i32 209, i32 100, i32 48, i32 3,
	i32 68, i32 203, i32 81, i32 110, i32 46, i32 84, i32 95, i32 214,
	i32 218, i32 30, i32 125, i32 185, i32 120, i32 55, i32 87, i32 144,
	i32 34, i32 209, i32 88, i32 32, i32 91, i32 169, i32 113, i32 151,
	i32 92, i32 83, i32 56, i32 109, i32 161, i32 155, i32 173, i32 83,
	i32 181, i32 24, i32 109, i32 217, i32 198, i32 202, i32 11, i32 74,
	i32 3, i32 75, i32 107, i32 4, i32 147, i32 162, i32 139, i32 8,
	i32 100, i32 79, i32 35, i32 191, i32 161, i32 178, i32 41, i32 189,
	i32 187, i32 105, i32 131, i32 186, i32 171, i32 215, i32 140, i32 25,
	i32 61, i32 125, i32 88, i32 99, i32 206, i32 137, i32 170, i32 13,
	i32 51, i32 85, i32 63, i32 167, i32 6, i32 179, i32 98, i32 131,
	i32 200, i32 135, i32 192, i32 210, i32 139, i32 15, i32 177, i32 61,
	i32 121, i32 158, i32 50, i32 76, i32 14, i32 181, i32 215, i32 129,
	i32 119, i32 190, i32 123, i32 75, i32 182, i32 138, i32 107, i32 101,
	i32 13, i32 146, i32 148, i32 19, i32 179, i32 28, i32 82, i32 90,
	i32 79, i32 134, i32 66, i32 101, i32 66, i32 111, i32 77, i32 12,
	i32 152, i32 38, i32 28, i32 24, i32 135, i32 193, i32 21, i32 158,
	i32 59, i32 115, i32 182, i32 50, i32 191, i32 27, i32 37, i32 91,
	i32 124, i32 104, i32 8, i32 17, i32 70, i32 136, i32 35, i32 14,
	i32 166, i32 27, i32 168, i32 132, i32 47, i32 145, i32 189, i32 169,
	i32 137, i32 117, i32 60, i32 67, i32 106, i32 213, i32 43, i32 123,
	i32 94, i32 97, i32 147, i32 39, i32 69, i32 42, i32 154, i32 184,
	i32 56, i32 5, i32 43, i32 59, i32 186, i32 55, i32 204, i32 149,
	i32 78, i32 121, i32 214, i32 135, i32 178, i32 57, i32 109, i32 140,
	i32 197, i32 141, i32 19, i32 50, i32 45, i32 197, i32 101, i32 206,
	i32 128, i32 67, i32 86, i32 110, i32 20, i32 33, i32 32, i32 31,
	i32 144, i32 44, i32 150, i32 203, i32 107, i32 76, i32 102, i32 92,
	i32 157, i32 11, i32 1, i32 27, i32 150, i32 71, i32 73, i32 71,
	i32 16, i32 68, i32 23, i32 79, i32 137, i32 128, i32 155, i32 51,
	i32 112, i32 26, i32 72, i32 207, i32 195, i32 93, i32 60, i32 29,
	i32 110, i32 106, i32 67, i32 196, i32 119, i32 113, i32 153, i32 26,
	i32 139, i32 183, i32 152, i32 192, i32 168, i32 196, i32 212, i32 81,
	i32 115, i32 103, i32 53, i32 126, i32 105, i32 22, i32 70, i32 64,
	i32 173, i32 159, i32 142, i32 62, i32 15, i32 155, i32 182, i32 106,
	i32 48, i32 197, i32 211, i32 33, i32 202, i32 29, i32 208, i32 136,
	i32 164, i32 134, i32 216, i32 169, i32 108, i32 36, i32 145, i32 188,
	i32 201, i32 192, i32 13, i32 90, i32 97, i32 20, i32 10, i32 153,
	i32 151, i32 64, i32 205, i32 146, i32 36, i32 215, i32 125, i32 32,
	i32 25, i32 211, i32 132, i32 49, i32 165, i32 68, i32 183, i32 8,
	i32 89, i32 2, i32 159, i32 2, i32 118, i32 7, i32 96, i32 180,
	i32 127, i32 95, i32 10, i32 138, i32 180, i32 157, i32 54, i32 93,
	i32 47, i32 127, i32 71, i32 25, i32 152, i32 118, i32 186, i32 184,
	i32 54, i32 108, i32 58, i32 136, i32 163, i32 111, i32 173, i32 9,
	i32 171, i32 190, i32 123, i32 199, i32 208, i32 131, i32 143, i32 58,
	i32 38, i32 30, i32 33, i32 56, i32 44, i32 52, i32 201, i32 121,
	i32 122, i32 42, i32 163, i32 89, i32 116, i32 45, i32 80, i32 140,
	i32 172, i32 184, i32 143, i32 105, i32 90, i32 0, i32 179, i32 193,
	i32 95, i32 171, i32 63, i32 170, i32 24, i32 55, i32 62, i32 108,
	i32 7, i32 115, i32 116, i32 60, i32 212, i32 64, i32 47, i32 61,
	i32 130, i32 40, i32 174, i32 40, i32 207, i32 157, i32 207, i32 156,
	i32 63, i32 187, i32 200, i32 42, i32 211, i32 203, i32 21, i32 51,
	i32 94, i32 99, i32 5, i32 1, i32 194, i32 2, i32 153, i32 181,
	i32 172, i32 164, i32 143, i32 163, i32 23, i32 126, i32 31, i32 193,
	i32 22, i32 17, i32 185, i32 218, i32 84, i32 89, i32 142, i32 147,
	i32 82, i32 86, i32 165, i32 158, i32 112, i32 205, i32 98, i32 191,
	i32 198, i32 78, i32 6, i32 93, i32 16, i32 160, i32 167, i32 76,
	i32 148, i32 29, i32 119, i32 213, i32 174, i32 156, i32 150, i32 72,
	i32 167, i32 99, i32 74, i32 70, i32 14, i32 7, i32 168, i32 41,
	i32 4, i32 39, i32 156, i32 176, i32 9, i32 114, i32 208, i32 20,
	i32 188, i32 65, i32 134, i32 164, i32 10, i32 204, i32 195, i32 159,
	i32 183, i32 111, i32 85, i32 103, i32 178, i32 199, i32 104, i32 38,
	i32 46
], align 4

@marshal_methods_number_of_classes = dso_local local_unnamed_addr constant i32 0, align 4

@marshal_methods_class_cache = dso_local local_unnamed_addr global [0 x %struct.MarshalMethodsManagedClass] zeroinitializer, align 8

; Names of classes in which marshal methods reside
@mm_class_names = dso_local local_unnamed_addr constant [0 x ptr] zeroinitializer, align 8

@mm_method_names = dso_local local_unnamed_addr constant [1 x %struct.MarshalMethodName] [
	%struct.MarshalMethodName {
		i64 u0x0000000000000000, ; name: 
		ptr @.MarshalMethodName.0_name; char* name
	} ; 0
], align 8

; get_function_pointer (uint32_t mono_image_index, uint32_t class_index, uint32_t method_token, void*& target_ptr)
@get_function_pointer = internal dso_local unnamed_addr global ptr null, align 8

; Functions

; Function attributes: memory(write, argmem: none, inaccessiblemem: none) "min-legal-vector-width"="0" mustprogress nofree norecurse nosync "no-trapping-math"="true" nounwind "stack-protector-buffer-size"="8" uwtable willreturn
define void @xamarin_app_init(ptr nocapture noundef readnone %env, ptr noundef %fn) local_unnamed_addr #0
{
	%fnIsNull = icmp eq ptr %fn, null
	br i1 %fnIsNull, label %1, label %2

1: ; preds = %0
	%putsResult = call noundef i32 @puts(ptr @.str.0)
	call void @abort()
	unreachable 

2: ; preds = %1, %0
	store ptr %fn, ptr @get_function_pointer, align 8, !tbaa !3
	ret void
}

; Strings
@.str.0 = private unnamed_addr constant [40 x i8] c"get_function_pointer MUST be specified\0A\00", align 1

;MarshalMethodName
@.MarshalMethodName.0_name = private unnamed_addr constant [1 x i8] c"\00", align 1

; External functions

; Function attributes: noreturn "no-trapping-math"="true" nounwind "stack-protector-buffer-size"="8"
declare void @abort() local_unnamed_addr #2

; Function attributes: nofree nounwind
declare noundef i32 @puts(ptr noundef) local_unnamed_addr #1
attributes #0 = { memory(write, argmem: none, inaccessiblemem: none) "min-legal-vector-width"="0" mustprogress nofree norecurse nosync "no-trapping-math"="true" nounwind "stack-protector-buffer-size"="8" "target-cpu"="generic" "target-features"="+fix-cortex-a53-835769,+neon,+outline-atomics,+v8a" uwtable willreturn }
attributes #1 = { nofree nounwind }
attributes #2 = { noreturn "no-trapping-math"="true" nounwind "stack-protector-buffer-size"="8" "target-cpu"="generic" "target-features"="+fix-cortex-a53-835769,+neon,+outline-atomics,+v8a" }

; Metadata
!llvm.module.flags = !{!0, !1, !7, !8, !9, !10}
!0 = !{i32 1, !"wchar_size", i32 4}
!1 = !{i32 7, !"PIC Level", i32 2}
!llvm.ident = !{!2}
!2 = !{!".NET for Android remotes/origin/release/9.0.1xx-rc2 @ 150245588b8014834bdf6ff8b7b282f86d8e2ea2"}
!3 = !{!4, !4, i64 0}
!4 = !{!"any pointer", !5, i64 0}
!5 = !{!"omnipotent char", !6, i64 0}
!6 = !{!"Simple C++ TBAA"}
!7 = !{i32 1, !"branch-target-enforcement", i32 0}
!8 = !{i32 1, !"sign-return-address", i32 0}
!9 = !{i32 1, !"sign-return-address-all", i32 0}
!10 = !{i32 1, !"sign-return-address-with-bkey", i32 0}
