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

@assembly_image_cache = dso_local local_unnamed_addr global [225 x ptr] zeroinitializer, align 8

; Each entry maps hash of an assembly name to an index into the `assembly_image_cache` array
@assembly_image_cache_hashes = dso_local local_unnamed_addr constant [675 x i64] [
	i64 u0x006b9d7c1c7e1c42, ; 0: de/Microsoft.Data.SqlClient.resources => 1
	i64 u0x0071cf2d27b7d61e, ; 1: lib_Xamarin.AndroidX.SwipeRefreshLayout.dll.so => 122
	i64 u0x01109b0e4d99e61f, ; 2: System.ComponentModel.Annotations.dll => 139
	i64 u0x02123411c4e01926, ; 3: lib_Xamarin.AndroidX.Navigation.Runtime.dll.so => 118
	i64 u0x022e81ea9c46e03a, ; 4: lib_CommunityToolkit.Maui.Core.dll.so => 52
	i64 u0x02827b47e97f2378, ; 5: System.Security.Cryptography.Pkcs.dll => 95
	i64 u0x029b2c18aaa0996c, ; 6: lib-ko-Microsoft.Data.SqlClient.resources.dll.so => 6
	i64 u0x02a4c5a44384f885, ; 7: Microsoft.Extensions.Caching.Memory => 63
	i64 u0x02abedc11addc1ed, ; 8: lib_Mono.Android.Runtime.dll.so => 223
	i64 u0x032267b2a94db371, ; 9: lib_Xamarin.AndroidX.AppCompat.dll.so => 100
	i64 u0x03621c804933a890, ; 10: System.Buffers => 133
	i64 u0x0399610510a38a38, ; 11: lib_System.Private.DataContractSerialization.dll.so => 176
	i64 u0x043032f1d071fae0, ; 12: ru/Microsoft.Maui.Controls.resources => 37
	i64 u0x044440a55165631e, ; 13: lib-cs-Microsoft.Maui.Controls.resources.dll.so => 15
	i64 u0x046eb1581a80c6b0, ; 14: vi/Microsoft.Maui.Controls.resources => 43
	i64 u0x0470607fd33c32db, ; 15: Microsoft.IdentityModel.Abstractions.dll => 74
	i64 u0x0517ef04e06e9f76, ; 16: System.Net.Primitives => 168
	i64 u0x0565d18c6da3de38, ; 17: Xamarin.AndroidX.RecyclerView => 120
	i64 u0x0581db89237110e9, ; 18: lib_System.Collections.dll.so => 138
	i64 u0x05989cb940b225a9, ; 19: Microsoft.Maui.dll => 82
	i64 u0x05a1c25e78e22d87, ; 20: lib_System.Runtime.CompilerServices.Unsafe.dll.so => 183
	i64 u0x05d8ca8ee551619f, ; 21: zh-Hant/Microsoft.Data.SqlClient.resources => 12
	i64 u0x06073ed944b92dc4, ; 22: lib-tr-Microsoft.Data.SqlClient.resources.dll.so => 10
	i64 u0x06076b5d2b581f08, ; 23: zh-HK/Microsoft.Maui.Controls.resources => 44
	i64 u0x06388ffe9f6c161a, ; 24: System.Xml.Linq.dll => 215
	i64 u0x0680a433c781bb3d, ; 25: Xamarin.AndroidX.Collection.Jvm => 104
	i64 u0x07c57877c7ba78ad, ; 26: ru/Microsoft.Maui.Controls.resources.dll => 37
	i64 u0x07dcdc7460a0c5e4, ; 27: System.Collections.NonGeneric => 136
	i64 u0x08015600dcbf6dc7, ; 28: it/Microsoft.Data.SqlClient.resources.dll => 4
	i64 u0x08881a0a9768df86, ; 29: lib_Azure.Core.dll.so => 48
	i64 u0x08a7c865576bbde7, ; 30: System.Reflection.Primitives => 182
	i64 u0x08f3c9788ee2153c, ; 31: Xamarin.AndroidX.DrawerLayout => 109
	i64 u0x09138715c92dba90, ; 32: lib_System.ComponentModel.Annotations.dll.so => 139
	i64 u0x0919c28b89381a0b, ; 33: lib_Microsoft.Extensions.Options.dll.so => 70
	i64 u0x092266563089ae3e, ; 34: lib_System.Collections.NonGeneric.dll.so => 136
	i64 u0x095cacaf6b6a32e4, ; 35: System.Memory.Data => 94
	i64 u0x09d144a7e214d457, ; 36: System.Security.Cryptography => 201
	i64 u0x0a805f95d98f597b, ; 37: lib_Microsoft.Extensions.Caching.Abstractions.dll.so => 62
	i64 u0x0abb3e2b271edc45, ; 38: System.Threading.Channels.dll => 208
	i64 u0x0adeb6c0f5699d33, ; 39: Microsoft.Data.SqlClient.dll => 58
	i64 u0x0b3b632c3bbee20c, ; 40: sk/Microsoft.Maui.Controls.resources => 38
	i64 u0x0b6aff547b84fbe9, ; 41: Xamarin.KotlinX.Serialization.Core.Jvm => 128
	i64 u0x0be2e1f8ce4064ed, ; 42: Xamarin.AndroidX.ViewPager => 123
	i64 u0x0c3ca6cc978e2aae, ; 43: pt-BR/Microsoft.Maui.Controls.resources => 34
	i64 u0x0c59ad9fbbd43abe, ; 44: Mono.Android => 224
	i64 u0x0c7790f60165fc06, ; 45: lib_Microsoft.Maui.Essentials.dll.so => 83
	i64 u0x0d3b5ab8b2766190, ; 46: lib_Microsoft.Bcl.AsyncInterfaces.dll.so => 57
	i64 u0x0e14e73a54dda68e, ; 47: lib_System.Net.NameResolution.dll.so => 166
	i64 u0x0fbe06392ef90569, ; 48: lib-ja-Microsoft.Data.SqlClient.resources.dll.so => 5
	i64 u0x102861e4055f511a, ; 49: Microsoft.Bcl.AsyncInterfaces.dll => 57
	i64 u0x102a31b45304b1da, ; 50: Xamarin.AndroidX.CustomView => 108
	i64 u0x108cf0e0ba098a51, ; 51: es/Microsoft.Data.SqlClient.resources => 2
	i64 u0x10f6cfcbcf801616, ; 52: System.IO.Compression.Brotli => 155
	i64 u0x114443cdcf2091f1, ; 53: System.Security.Cryptography.Primitives => 199
	i64 u0x123639456fb056da, ; 54: System.Reflection.Emit.Lightweight.dll => 181
	i64 u0x124f38a5d8cb5fb8, ; 55: K4os.Compression.LZ4.dll => 54
	i64 u0x125b7f94acb989db, ; 56: Xamarin.AndroidX.RecyclerView.dll => 120
	i64 u0x126ee4b0de53cbfd, ; 57: Microsoft.IdentityModel.Protocols.OpenIdConnect.dll => 78
	i64 u0x138567fa954faa55, ; 58: Xamarin.AndroidX.Browser => 102
	i64 u0x13a01de0cbc3f06c, ; 59: lib-fr-Microsoft.Maui.Controls.resources.dll.so => 21
	i64 u0x13f1e5e209e91af4, ; 60: lib_Java.Interop.dll.so => 222
	i64 u0x13f1e880c25d96d1, ; 61: he/Microsoft.Maui.Controls.resources => 22
	i64 u0x143a1f6e62b82b56, ; 62: Microsoft.IdentityModel.Protocols.OpenIdConnect => 78
	i64 u0x143d8ea60a6a4011, ; 63: Microsoft.Extensions.DependencyInjection.Abstractions => 67
	i64 u0x152a448bd1e745a7, ; 64: Microsoft.Win32.Primitives => 132
	i64 u0x159cc6c81072f00e, ; 65: lib_System.Diagnostics.EventLog.dll.so => 92
	i64 u0x162be8a76b00cd97, ; 66: lib-de-Microsoft.Data.SqlClient.resources.dll.so => 1
	i64 u0x16bf2a22df043a09, ; 67: System.IO.Pipes.dll => 160
	i64 u0x16ea2b318ad2d830, ; 68: System.Security.Cryptography.Algorithms => 196
	i64 u0x17125c9a85b4929f, ; 69: lib_netstandard.dll.so => 220
	i64 u0x1716866f7416792e, ; 70: lib_System.Security.AccessControl.dll.so => 194
	i64 u0x17b56e25558a5d36, ; 71: lib-hu-Microsoft.Maui.Controls.resources.dll.so => 25
	i64 u0x17f9358913beb16a, ; 72: System.Text.Encodings.Web => 205
	i64 u0x18402a709e357f3b, ; 73: lib_Xamarin.KotlinX.Serialization.Core.Jvm.dll.so => 128
	i64 u0x18a9befae51bb361, ; 74: System.Net.WebClient => 172
	i64 u0x18f0ce884e87d89a, ; 75: nb/Microsoft.Maui.Controls.resources.dll => 31
	i64 u0x19a4c090f14ebb66, ; 76: System.Security.Claims => 195
	i64 u0x1a6fceea64859810, ; 77: Azure.Identity => 49
	i64 u0x1a91866a319e9259, ; 78: lib_System.Collections.Concurrent.dll.so => 134
	i64 u0x1aac34d1917ba5d3, ; 79: lib_System.dll.so => 219
	i64 u0x1aad60783ffa3e5b, ; 80: lib-th-Microsoft.Maui.Controls.resources.dll.so => 40
	i64 u0x1c753b5ff15bce1b, ; 81: Mono.Android.Runtime.dll => 223
	i64 u0x1db6820994506bf5, ; 82: System.IO.FileSystem.AccessControl.dll => 157
	i64 u0x1dba6509cc55b56f, ; 83: lib_Google.Protobuf.dll.so => 53
	i64 u0x1dbb0c2c6a999acb, ; 84: System.Diagnostics.StackTrace => 148
	i64 u0x1e3d87657e9659bc, ; 85: Xamarin.AndroidX.Navigation.UI => 119
	i64 u0x1e71143913d56c10, ; 86: lib-ko-Microsoft.Maui.Controls.resources.dll.so => 29
	i64 u0x1ed8fcce5e9b50a0, ; 87: Microsoft.Extensions.Options.dll => 70
	i64 u0x1f055d15d807e1b2, ; 88: System.Xml.XmlSerializer => 218
	i64 u0x20237ea48006d7a8, ; 89: lib_System.Net.WebClient.dll.so => 172
	i64 u0x209375905fcc1bad, ; 90: lib_System.IO.Compression.Brotli.dll.so => 155
	i64 u0x20edad43b59fbd8e, ; 91: System.Security.Permissions.dll => 97
	i64 u0x20fab3cf2dfbc8df, ; 92: lib_System.Diagnostics.Process.dll.so => 147
	i64 u0x2174319c0d835bc9, ; 93: System.Runtime => 193
	i64 u0x2199f06354c82d3b, ; 94: System.ClientModel.dll => 89
	i64 u0x21cc7e445dcd5469, ; 95: System.Reflection.Emit.ILGeneration => 180
	i64 u0x220fd4f2e7c48170, ; 96: th/Microsoft.Maui.Controls.resources => 40
	i64 u0x224538d85ed15a82, ; 97: System.IO.Pipes => 160
	i64 u0x234b2420fe4b9bdc, ; 98: lib_K4os.Compression.LZ4.dll.so => 54
	i64 u0x237be844f1f812c7, ; 99: System.Threading.Thread.dll => 210
	i64 u0x23807c59646ec4f3, ; 100: lib_Microsoft.EntityFrameworkCore.dll.so => 59
	i64 u0x23cce13de11e9adc, ; 101: Lotus Spor.dll => 130
	i64 u0x23f599165f90dd7a, ; 102: lib-cs-Microsoft.Data.SqlClient.resources.dll.so => 0
	i64 u0x2407aef2bbe8fadf, ; 103: System.Console => 144
	i64 u0x240abe014b27e7d3, ; 104: Xamarin.AndroidX.Core.dll => 106
	i64 u0x247619fe4413f8bf, ; 105: System.Runtime.Serialization.Primitives.dll => 191
	i64 u0x252073cc3caa62c2, ; 106: fr/Microsoft.Maui.Controls.resources.dll => 21
	i64 u0x2662c629b96b0b30, ; 107: lib_Xamarin.Kotlin.StdLib.dll.so => 126
	i64 u0x268c1439f13bcc29, ; 108: lib_Microsoft.Extensions.Primitives.dll.so => 71
	i64 u0x270a44600c921861, ; 109: System.IdentityModel.Tokens.Jwt => 93
	i64 u0x273f3515de5faf0d, ; 110: id/Microsoft.Maui.Controls.resources.dll => 26
	i64 u0x2742545f9094896d, ; 111: hr/Microsoft.Maui.Controls.resources => 24
	i64 u0x27b410442fad6cf1, ; 112: Java.Interop.dll => 222
	i64 u0x2801845a2c71fbfb, ; 113: System.Net.Primitives.dll => 168
	i64 u0x28e27b6a43116264, ; 114: lib_Lotus Spor.dll.so => 130
	i64 u0x2a128783efe70ba0, ; 115: uk/Microsoft.Maui.Controls.resources.dll => 42
	i64 u0x2a3b095612184159, ; 116: lib_System.Net.NetworkInformation.dll.so => 167
	i64 u0x2a6507a5ffabdf28, ; 117: System.Diagnostics.TraceSource.dll => 150
	i64 u0x2ad156c8e1354139, ; 118: fi/Microsoft.Maui.Controls.resources => 20
	i64 u0x2af298f63581d886, ; 119: System.Text.RegularExpressions.dll => 207
	i64 u0x2af615542f04da50, ; 120: System.IdentityModel.Tokens.Jwt.dll => 93
	i64 u0x2afc1c4f898552ee, ; 121: lib_System.Formats.Asn1.dll.so => 154
	i64 u0x2b148910ed40fbf9, ; 122: zh-Hant/Microsoft.Maui.Controls.resources.dll => 46
	i64 u0x2c8bd14bb93a7d82, ; 123: lib-pl-Microsoft.Maui.Controls.resources.dll.so => 33
	i64 u0x2cbd9262ca785540, ; 124: lib_System.Text.Encoding.CodePages.dll.so => 203
	i64 u0x2cc9e1fed6257257, ; 125: lib_System.Reflection.Emit.Lightweight.dll.so => 181
	i64 u0x2cd723e9fe623c7c, ; 126: lib_System.Private.Xml.Linq.dll.so => 178
	i64 u0x2ce66f4c8733e883, ; 127: pt-BR/Microsoft.Data.SqlClient.resources.dll => 8
	i64 u0x2d169d318a968379, ; 128: System.Threading.dll => 212
	i64 u0x2d47774b7d993f59, ; 129: sv/Microsoft.Maui.Controls.resources.dll => 39
	i64 u0x2db915caf23548d2, ; 130: System.Text.Json.dll => 206
	i64 u0x2e6f1f226821322a, ; 131: el/Microsoft.Maui.Controls.resources.dll => 18
	i64 u0x2f02f94df3200fe5, ; 132: System.Diagnostics.Process => 147
	i64 u0x2f2e98e1c89b1aff, ; 133: System.Xml.ReaderWriter => 216
	i64 u0x2f40b2521deba305, ; 134: lib_Microsoft.SqlServer.Server.dll.so => 85
	i64 u0x2f5911d9ba814e4e, ; 135: System.Diagnostics.Tracing => 151
	i64 u0x2feb4d2fcda05cfd, ; 136: Microsoft.Extensions.Caching.Abstractions.dll => 62
	i64 u0x309ee9eeec09a71e, ; 137: lib_Xamarin.AndroidX.Fragment.dll.so => 110
	i64 u0x309f2bedefa9a318, ; 138: Microsoft.IdentityModel.Abstractions => 74
	i64 u0x31195fef5d8fb552, ; 139: _Microsoft.Android.Resource.Designer.dll => 47
	i64 u0x31a267c1617036d6, ; 140: MySql.EntityFrameworkCore.dll => 87
	i64 u0x32243413e774362a, ; 141: Xamarin.AndroidX.CardView.dll => 103
	i64 u0x3235427f8d12dae1, ; 142: lib_System.Drawing.Primitives.dll.so => 152
	i64 u0x329753a17a517811, ; 143: fr/Microsoft.Maui.Controls.resources => 21
	i64 u0x32aa989ff07a84ff, ; 144: lib_System.Xml.ReaderWriter.dll.so => 216
	i64 u0x33829542f112d59b, ; 145: System.Collections.Immutable => 135
	i64 u0x33a31443733849fe, ; 146: lib-es-Microsoft.Maui.Controls.resources.dll.so => 19
	i64 u0x341abc357fbb4ebf, ; 147: lib_System.Net.Sockets.dll.so => 171
	i64 u0x348d598f4054415e, ; 148: Microsoft.SqlServer.Server => 85
	i64 u0x34dfd74fe2afcf37, ; 149: Microsoft.Maui => 82
	i64 u0x34e292762d9615df, ; 150: cs/Microsoft.Maui.Controls.resources.dll => 15
	i64 u0x3508234247f48404, ; 151: Microsoft.Maui.Controls => 80
	i64 u0x353590da528c9d22, ; 152: System.ComponentModel.Annotations => 139
	i64 u0x3549870798b4cd30, ; 153: lib_Xamarin.AndroidX.ViewPager2.dll.so => 124
	i64 u0x355282fc1c909694, ; 154: Microsoft.Extensions.Configuration => 64
	i64 u0x355c649948d55d97, ; 155: lib_System.Runtime.Intrinsics.dll.so => 186
	i64 u0x36b2b50fdf589ae2, ; 156: System.Reflection.Emit.Lightweight => 181
	i64 u0x374ef46b06791af6, ; 157: System.Reflection.Primitives.dll => 182
	i64 u0x380134e03b1e160a, ; 158: System.Collections.Immutable.dll => 135
	i64 u0x38049b5c59b39324, ; 159: System.Runtime.CompilerServices.Unsafe => 183
	i64 u0x385c17636bb6fe6e, ; 160: Xamarin.AndroidX.CustomView.dll => 108
	i64 u0x38869c811d74050e, ; 161: System.Net.NameResolution.dll => 166
	i64 u0x38e93ec1c057cdf6, ; 162: Microsoft.IdentityModel.Protocols => 77
	i64 u0x39251dccb84bdcaa, ; 163: lib_System.Configuration.ConfigurationManager.dll.so => 90
	i64 u0x393c226616977fdb, ; 164: lib_Xamarin.AndroidX.ViewPager.dll.so => 123
	i64 u0x395e37c3334cf82a, ; 165: lib-ca-Microsoft.Maui.Controls.resources.dll.so => 14
	i64 u0x39aa39fda111d9d3, ; 166: Newtonsoft.Json => 88
	i64 u0x3ab5859054645f72, ; 167: System.Security.Cryptography.Primitives.dll => 199
	i64 u0x3b2c47fe17204e4d, ; 168: MySql.Data => 86
	i64 u0x3b860f9932505633, ; 169: lib_System.Text.Encoding.Extensions.dll.so => 204
	i64 u0x3bea9ebe8c027c01, ; 170: lib_Microsoft.IdentityModel.Tokens.dll.so => 79
	i64 u0x3c3aafb6b3a00bf6, ; 171: lib_System.Security.Cryptography.X509Certificates.dll.so => 200
	i64 u0x3c5f19e4acdcebd8, ; 172: lib_Microsoft.Data.SqlClient.dll.so => 58
	i64 u0x3c7c495f58ac5ee9, ; 173: Xamarin.Kotlin.StdLib => 126
	i64 u0x3cd9d281d402eb9b, ; 174: Xamarin.AndroidX.Browser.dll => 102
	i64 u0x3d196e782ed8c01a, ; 175: System.Data.SqlClient => 91
	i64 u0x3d2b1913edfc08d7, ; 176: lib_System.Threading.ThreadPool.dll.so => 211
	i64 u0x3d46f0b995082740, ; 177: System.Xml.Linq => 215
	i64 u0x3d9c2a242b040a50, ; 178: lib_Xamarin.AndroidX.Core.dll.so => 106
	i64 u0x3daa14724d8f58e8, ; 179: Google.Protobuf.dll => 53
	i64 u0x3e0b360b2840f096, ; 180: it/Microsoft.Data.SqlClient.resources => 4
	i64 u0x3f3c8f45ab6f28c7, ; 181: Microsoft.Identity.Client.Extensions.Msal.dll => 73
	i64 u0x3f510adf788828dd, ; 182: System.Threading.Tasks.Extensions => 209
	i64 u0x4019503dd3d938a1, ; 183: MySql.Data.dll => 86
	i64 u0x407a10bb4bf95829, ; 184: lib_Xamarin.AndroidX.Navigation.Common.dll.so => 116
	i64 u0x407ac43dee26bd5a, ; 185: lib_Azure.Identity.dll.so => 49
	i64 u0x415e36f6b13ff6f3, ; 186: System.Configuration.ConfigurationManager.dll => 90
	i64 u0x41cab042be111c34, ; 187: lib_Xamarin.AndroidX.AppCompat.AppCompatResources.dll.so => 101
	i64 u0x43375950ec7c1b6a, ; 188: netstandard.dll => 220
	i64 u0x434c4e1d9284cdae, ; 189: Mono.Android.dll => 224
	i64 u0x43950f84de7cc79a, ; 190: pl/Microsoft.Maui.Controls.resources.dll => 33
	i64 u0x448bd33429269b19, ; 191: Microsoft.CSharp => 131
	i64 u0x4499fa3c8e494654, ; 192: lib_System.Runtime.Serialization.Primitives.dll.so => 191
	i64 u0x4515080865a951a5, ; 193: Xamarin.Kotlin.StdLib.dll => 126
	i64 u0x453c1277f85cf368, ; 194: lib_Microsoft.EntityFrameworkCore.Abstractions.dll.so => 60
	i64 u0x458d2df79ac57c1d, ; 195: lib_System.IdentityModel.Tokens.Jwt.dll.so => 93
	i64 u0x45c40276a42e283e, ; 196: System.Diagnostics.TraceSource => 150
	i64 u0x46a4213bc97fe5ae, ; 197: lib-ru-Microsoft.Maui.Controls.resources.dll.so => 37
	i64 u0x47358bd471172e1d, ; 198: lib_System.Xml.Linq.dll.so => 215
	i64 u0x4787a936949fcac2, ; 199: System.Memory.Data.dll => 94
	i64 u0x47daf4e1afbada10, ; 200: pt/Microsoft.Maui.Controls.resources => 35
	i64 u0x4953c088b9debf0a, ; 201: lib_System.Security.Permissions.dll.so => 97
	i64 u0x49e952f19a4e2022, ; 202: System.ObjectModel => 175
	i64 u0x4a5667b2462a664b, ; 203: lib_Xamarin.AndroidX.Navigation.UI.dll.so => 119
	i64 u0x4b576d47ac054f3c, ; 204: System.IO.FileSystem.AccessControl => 157
	i64 u0x4b7b6532ded934b7, ; 205: System.Text.Json => 206
	i64 u0x4b8f8ea3c2df6bb0, ; 206: System.ClientModel => 89
	i64 u0x4ca014ceac582c86, ; 207: Microsoft.EntityFrameworkCore.Relational.dll => 61
	i64 u0x4cc5f15266470798, ; 208: lib_Xamarin.AndroidX.Loader.dll.so => 115
	i64 u0x4cf6f67dc77aacd2, ; 209: System.Net.NetworkInformation.dll => 167
	i64 u0x4d479f968a05e504, ; 210: System.Linq.Expressions.dll => 161
	i64 u0x4d55a010ffc4faff, ; 211: System.Private.Xml => 179
	i64 u0x4d6001db23f8cd87, ; 212: lib_System.ClientModel.dll.so => 89
	i64 u0x4d95fccc1f67c7ca, ; 213: System.Runtime.Loader.dll => 187
	i64 u0x4dcf44c3c9b076a2, ; 214: it/Microsoft.Maui.Controls.resources.dll => 27
	i64 u0x4dd9247f1d2c3235, ; 215: Xamarin.AndroidX.Loader.dll => 115
	i64 u0x4e32f00cb0937401, ; 216: Mono.Android.Runtime => 223
	i64 u0x4e5eea4668ac2b18, ; 217: System.Text.Encoding.CodePages => 203
	i64 u0x4ebd0c4b82c5eefc, ; 218: lib_System.Threading.Channels.dll.so => 208
	i64 u0x4f21ee6ef9eb527e, ; 219: ca/Microsoft.Maui.Controls.resources => 14
	i64 u0x4f27ca9d6e02176c, ; 220: cs/Microsoft.Data.SqlClient.resources => 0
	i64 u0x4ffd65baff757598, ; 221: Microsoft.IdentityModel.Tokens => 79
	i64 u0x50320f2a19424f3f, ; 222: lib-it-Microsoft.Data.SqlClient.resources.dll.so => 4
	i64 u0x5037f0be3c28c7a3, ; 223: lib_Microsoft.Maui.Controls.dll.so => 80
	i64 u0x5131bbe80989093f, ; 224: Xamarin.AndroidX.Lifecycle.ViewModel.Android.dll => 113
	i64 u0x5146d4e23aed3198, ; 225: ja/Microsoft.Data.SqlClient.resources => 5
	i64 u0x51bb8a2afe774e32, ; 226: System.Drawing => 153
	i64 u0x526ce79eb8e90527, ; 227: lib_System.Net.Primitives.dll.so => 168
	i64 u0x52829f00b4467c38, ; 228: lib_System.Data.Common.dll.so => 145
	i64 u0x5290402954d7bce0, ; 229: zh-Hans/Microsoft.Data.SqlClient.resources => 11
	i64 u0x529ffe06f39ab8db, ; 230: Xamarin.AndroidX.Core => 106
	i64 u0x52ff996554dbf352, ; 231: Microsoft.Maui.Graphics => 84
	i64 u0x535f7e40e8fef8af, ; 232: lib-sk-Microsoft.Maui.Controls.resources.dll.so => 38
	i64 u0x53a96d5c86c9e194, ; 233: System.Net.NetworkInformation => 167
	i64 u0x53be1038a61e8d44, ; 234: System.Runtime.InteropServices.RuntimeInformation.dll => 184
	i64 u0x53c3014b9437e684, ; 235: lib-zh-HK-Microsoft.Maui.Controls.resources.dll.so => 44
	i64 u0x5435e6f049e9bc37, ; 236: System.Security.Claims.dll => 195
	i64 u0x54795225dd1587af, ; 237: lib_System.Runtime.dll.so => 193
	i64 u0x556e8b63b660ab8b, ; 238: Xamarin.AndroidX.Lifecycle.Common.Jvm.dll => 111
	i64 u0x5588627c9a108ec9, ; 239: System.Collections.Specialized => 137
	i64 u0x56442b99bc64bb47, ; 240: System.Runtime.Serialization.Xml.dll => 192
	i64 u0x571c5cfbec5ae8e2, ; 241: System.Private.Uri => 177
	i64 u0x579a06fed6eec900, ; 242: System.Private.CoreLib.dll => 221
	i64 u0x57c542c14049b66d, ; 243: System.Diagnostics.DiagnosticSource => 146
	i64 u0x58601b2dda4a27b9, ; 244: lib-ja-Microsoft.Maui.Controls.resources.dll.so => 28
	i64 u0x58688d9af496b168, ; 245: Microsoft.Extensions.DependencyInjection.dll => 66
	i64 u0x595a356d23e8da9a, ; 246: lib_Microsoft.CSharp.dll.so => 131
	i64 u0x5a70033ca9d003cb, ; 247: lib_System.Memory.Data.dll.so => 94
	i64 u0x5a89a886ae30258d, ; 248: lib_Xamarin.AndroidX.CoordinatorLayout.dll.so => 105
	i64 u0x5a8f6699f4a1caa9, ; 249: lib_System.Threading.dll.so => 212
	i64 u0x5ae9cd33b15841bf, ; 250: System.ComponentModel => 143
	i64 u0x5b54391bdc6fcfe6, ; 251: System.Private.DataContractSerialization => 176
	i64 u0x5b5f0e240a06a2a2, ; 252: da/Microsoft.Maui.Controls.resources.dll => 16
	i64 u0x5b608c01082a90a8, ; 253: K4os.Hash.xxHash => 56
	i64 u0x5bf46332cc09e9b2, ; 254: lib_System.Data.SqlClient.dll.so => 91
	i64 u0x5c393624b8176517, ; 255: lib_Microsoft.Extensions.Logging.dll.so => 68
	i64 u0x5d0a4a29b02d9d3c, ; 256: System.Net.WebHeaderCollection.dll => 173
	i64 u0x5d33da2f84c1de97, ; 257: lib-pt-BR-Microsoft.Data.SqlClient.resources.dll.so => 8
	i64 u0x5d7960d446a1890e, ; 258: lib-pl-Microsoft.Data.SqlClient.resources.dll.so => 7
	i64 u0x5db0cbbd1028510e, ; 259: lib_System.Runtime.InteropServices.dll.so => 185
	i64 u0x5db30905d3e5013b, ; 260: Xamarin.AndroidX.Collection.Jvm.dll => 104
	i64 u0x5e467bc8f09ad026, ; 261: System.Collections.Specialized.dll => 137
	i64 u0x5ea92fdb19ec8c4c, ; 262: System.Text.Encodings.Web.dll => 205
	i64 u0x5eb8046dd40e9ac3, ; 263: System.ComponentModel.Primitives => 141
	i64 u0x5ec272d219c9aba4, ; 264: System.Security.Cryptography.Csp.dll => 197
	i64 u0x5f36ccf5c6a57e24, ; 265: System.Xml.ReaderWriter.dll => 216
	i64 u0x5f4294b9b63cb842, ; 266: System.Data.Common => 145
	i64 u0x5f9a2d823f664957, ; 267: lib-el-Microsoft.Maui.Controls.resources.dll.so => 18
	i64 u0x5fac98e0b37a5b9d, ; 268: System.Runtime.CompilerServices.Unsafe.dll => 183
	i64 u0x609f4b7b63d802d4, ; 269: lib_Microsoft.Extensions.DependencyInjection.dll.so => 66
	i64 u0x60cd4e33d7e60134, ; 270: Xamarin.KotlinX.Coroutines.Core.Jvm => 127
	i64 u0x60f62d786afcf130, ; 271: System.Memory => 164
	i64 u0x618073e67851e2a7, ; 272: lib_K4os.Compression.LZ4.Streams.dll.so => 55
	i64 u0x61be8d1299194243, ; 273: Microsoft.Maui.Controls.Xaml => 81
	i64 u0x61d2cba29557038f, ; 274: de/Microsoft.Maui.Controls.resources => 17
	i64 u0x61d88f399afb2f45, ; 275: lib_System.Runtime.Loader.dll.so => 187
	i64 u0x6219beeff33faa04, ; 276: cs/Microsoft.Data.SqlClient.resources.dll => 0
	i64 u0x622eef6f9e59068d, ; 277: System.Private.CoreLib => 221
	i64 u0x625def565caafc1c, ; 278: tr/Microsoft.Data.SqlClient.resources.dll => 10
	i64 u0x63f1f6883c1e23c2, ; 279: lib_System.Collections.Immutable.dll.so => 135
	i64 u0x6400f68068c1e9f1, ; 280: Xamarin.Google.Android.Material.dll => 125
	i64 u0x640e3b14dbd325c2, ; 281: System.Security.Cryptography.Algorithms.dll => 196
	i64 u0x658f524e4aba7dad, ; 282: CommunityToolkit.Maui.dll => 51
	i64 u0x65a51fb1cf95ad53, ; 283: ZstdSharp.dll => 129
	i64 u0x65ecac39144dd3cc, ; 284: Microsoft.Maui.Controls.dll => 80
	i64 u0x65ece51227bfa724, ; 285: lib_System.Runtime.Numerics.dll.so => 188
	i64 u0x6692e924eade1b29, ; 286: lib_System.Console.dll.so => 144
	i64 u0x66a4e5c6a3fb0bae, ; 287: lib_Xamarin.AndroidX.Lifecycle.ViewModel.Android.dll.so => 113
	i64 u0x66d13304ce1a3efa, ; 288: Xamarin.AndroidX.CursorAdapter => 107
	i64 u0x68558ec653afa616, ; 289: lib-da-Microsoft.Maui.Controls.resources.dll.so => 16
	i64 u0x6872ec7a2e36b1ac, ; 290: System.Drawing.Primitives.dll => 152
	i64 u0x68fbbbe2eb455198, ; 291: System.Formats.Asn1 => 154
	i64 u0x69063fc0ba8e6bdd, ; 292: he/Microsoft.Maui.Controls.resources.dll => 22
	i64 u0x6a4d7577b2317255, ; 293: System.Runtime.InteropServices.dll => 185
	i64 u0x6ace3b74b15ee4a4, ; 294: nb/Microsoft.Maui.Controls.resources => 31
	i64 u0x6b2e67033d722856, ; 295: MySql.EntityFrameworkCore => 87
	i64 u0x6d0a12b2adba20d8, ; 296: System.Security.Cryptography.ProtectedData.dll => 96
	i64 u0x6d12bfaa99c72b1f, ; 297: lib_Microsoft.Maui.Graphics.dll.so => 84
	i64 u0x6d3b7628f8253e93, ; 298: pl/Microsoft.Data.SqlClient.resources => 7
	i64 u0x6d70755158ca866e, ; 299: lib_System.ComponentModel.EventBasedAsync.dll.so => 140
	i64 u0x6d79993361e10ef2, ; 300: Microsoft.Extensions.Primitives => 71
	i64 u0x6d86d56b84c8eb71, ; 301: lib_Xamarin.AndroidX.CursorAdapter.dll.so => 107
	i64 u0x6d9bea6b3e895cf7, ; 302: Microsoft.Extensions.Primitives.dll => 71
	i64 u0x6e25a02c3833319a, ; 303: lib_Xamarin.AndroidX.Navigation.Fragment.dll.so => 117
	i64 u0x6fd2265da78b93a4, ; 304: lib_Microsoft.Maui.dll.so => 82
	i64 u0x6fdfc7de82c33008, ; 305: cs/Microsoft.Maui.Controls.resources => 15
	i64 u0x6ffc4967cc47ba57, ; 306: System.IO.FileSystem.Watcher.dll => 158
	i64 u0x70e99f48c05cb921, ; 307: tr/Microsoft.Maui.Controls.resources.dll => 41
	i64 u0x70fd3deda22442d2, ; 308: lib-nb-Microsoft.Maui.Controls.resources.dll.so => 31
	i64 u0x71a495ea3761dde8, ; 309: lib-it-Microsoft.Maui.Controls.resources.dll.so => 27
	i64 u0x71ad672adbe48f35, ; 310: System.ComponentModel.Primitives.dll => 141
	i64 u0x71bc142d620e986a, ; 311: lib_System.Security.Cryptography.Pkcs.dll.so => 95
	i64 u0x725f5a9e82a45c81, ; 312: System.Security.Cryptography.Encoding => 198
	i64 u0x72b1fb4109e08d7b, ; 313: lib-hr-Microsoft.Maui.Controls.resources.dll.so => 24
	i64 u0x73e4ce94e2eb6ffc, ; 314: lib_System.Memory.dll.so => 164
	i64 u0x755a91767330b3d4, ; 315: lib_Microsoft.Extensions.Configuration.dll.so => 64
	i64 u0x75ca35803663dc65, ; 316: VatanSoft.VatanSms.Net => 98
	i64 u0x76012e7334db86e5, ; 317: lib_Xamarin.AndroidX.SavedState.dll.so => 121
	i64 u0x76ca07b878f44da0, ; 318: System.Runtime.Numerics.dll => 188
	i64 u0x777b4ed432c1e61e, ; 319: K4os.Compression.LZ4.Streams => 55
	i64 u0x780bc73597a503a9, ; 320: lib-ms-Microsoft.Maui.Controls.resources.dll.so => 30
	i64 u0x783606d1e53e7a1a, ; 321: th/Microsoft.Maui.Controls.resources.dll => 40
	i64 u0x7841c47b741b9f64, ; 322: System.Security.Permissions => 97
	i64 u0x7891b42e012cde6f, ; 323: Lotus Spor => 130
	i64 u0x78a45e51311409b6, ; 324: Xamarin.AndroidX.Fragment.dll => 110
	i64 u0x79eb916f2d11e1f0, ; 325: zh-Hans/Microsoft.Data.SqlClient.resources.dll => 11
	i64 u0x7adb8da2ac89b647, ; 326: fi/Microsoft.Maui.Controls.resources.dll => 20
	i64 u0x7b4927e421291c41, ; 327: Microsoft.IdentityModel.JsonWebTokens.dll => 75
	i64 u0x7bef86a4335c4870, ; 328: System.ComponentModel.TypeConverter => 142
	i64 u0x7c0820144cd34d6a, ; 329: sk/Microsoft.Maui.Controls.resources.dll => 38
	i64 u0x7c2a0bd1e0f988fc, ; 330: lib-de-Microsoft.Maui.Controls.resources.dll.so => 17
	i64 u0x7c41d387501568ba, ; 331: System.Net.WebClient.dll => 172
	i64 u0x7cc637f941f716d0, ; 332: CommunityToolkit.Maui.Core => 52
	i64 u0x7d649b75d580bb42, ; 333: ms/Microsoft.Maui.Controls.resources.dll => 30
	i64 u0x7d8ee2bdc8e3aad1, ; 334: System.Numerics.Vectors => 174
	i64 u0x7dfc3d6d9d8d7b70, ; 335: System.Collections => 138
	i64 u0x7e1f8f575a3599cb, ; 336: BouncyCastle.Cryptography.dll => 50
	i64 u0x7e2e564fa2f76c65, ; 337: lib_System.Diagnostics.Tracing.dll.so => 151
	i64 u0x7e302e110e1e1346, ; 338: lib_System.Security.Claims.dll.so => 195
	i64 u0x7e946809d6008ef2, ; 339: lib_System.ObjectModel.dll.so => 175
	i64 u0x7ecc13347c8fd849, ; 340: lib_System.ComponentModel.dll.so => 143
	i64 u0x7f00ddd9b9ca5a13, ; 341: Xamarin.AndroidX.ViewPager.dll => 123
	i64 u0x7f9351cd44b1273f, ; 342: Microsoft.Extensions.Configuration.Abstractions => 65
	i64 u0x7fae0ef4dc4770fe, ; 343: Microsoft.Identity.Client => 72
	i64 u0x7fbd557c99b3ce6f, ; 344: lib_Xamarin.AndroidX.Lifecycle.LiveData.Core.dll.so => 112
	i64 u0x812c069d5cdecc17, ; 345: System.dll => 219
	i64 u0x81ab745f6c0f5ce6, ; 346: zh-Hant/Microsoft.Maui.Controls.resources => 46
	i64 u0x82075fdf49c26af2, ; 347: ZstdSharp => 129
	i64 u0x8277f2be6b5ce05f, ; 348: Xamarin.AndroidX.AppCompat => 100
	i64 u0x828f06563b30bc50, ; 349: lib_Xamarin.AndroidX.CardView.dll.so => 103
	i64 u0x82df8f5532a10c59, ; 350: lib_System.Drawing.dll.so => 153
	i64 u0x82f6403342e12049, ; 351: uk/Microsoft.Maui.Controls.resources => 42
	i64 u0x831b2537d5b3a2ed, ; 352: lib_VatanSoft.VatanSms.Net.dll.so => 98
	i64 u0x83a7afd2c49adc86, ; 353: lib_Microsoft.IdentityModel.Abstractions.dll.so => 74
	i64 u0x83c14ba66c8e2b8c, ; 354: zh-Hans/Microsoft.Maui.Controls.resources => 45
	i64 u0x84ae73148a4557d2, ; 355: lib_System.IO.Pipes.dll.so => 160
	i64 u0x84b01102c12a9232, ; 356: System.Runtime.Serialization.Json.dll => 190
	i64 u0x84cd5cdec0f54bcc, ; 357: lib_Microsoft.EntityFrameworkCore.Relational.dll.so => 61
	i64 u0x8528b82bdbc15371, ; 358: ko/Microsoft.Data.SqlClient.resources => 6
	i64 u0x86a909228dc7657b, ; 359: lib-zh-Hant-Microsoft.Maui.Controls.resources.dll.so => 46
	i64 u0x86b3e00c36b84509, ; 360: Microsoft.Extensions.Configuration.dll => 64
	i64 u0x86b62cb077ec4fd7, ; 361: System.Runtime.Serialization.Xml => 192
	i64 u0x87c4b8a492b176ad, ; 362: Microsoft.EntityFrameworkCore.Abstractions => 60
	i64 u0x87c69b87d9283884, ; 363: lib_System.Threading.Thread.dll.so => 210
	i64 u0x87f6569b25707834, ; 364: System.IO.Compression.Brotli.dll => 155
	i64 u0x8842b3a5d2d3fb36, ; 365: Microsoft.Maui.Essentials => 83
	i64 u0x88bda98e0cffb7a9, ; 366: lib_Xamarin.KotlinX.Coroutines.Core.Jvm.dll.so => 127
	i64 u0x8930322c7bd8f768, ; 367: netstandard => 220
	i64 u0x897a606c9e39c75f, ; 368: lib_System.ComponentModel.Primitives.dll.so => 141
	i64 u0x89c5188089ec2cd5, ; 369: lib_System.Runtime.InteropServices.RuntimeInformation.dll.so => 184
	i64 u0x8a399a706fcbce4b, ; 370: Microsoft.Extensions.Caching.Abstractions => 62
	i64 u0x8ad229ea26432ee2, ; 371: Xamarin.AndroidX.Loader => 115
	i64 u0x8b4ff5d0fdd5faa1, ; 372: lib_System.Diagnostics.DiagnosticSource.dll.so => 146
	i64 u0x8b541d476eb3774c, ; 373: System.Security.Principal.Windows => 202
	i64 u0x8b8d01333a96d0b5, ; 374: System.Diagnostics.Process.dll => 147
	i64 u0x8b9ceca7acae3451, ; 375: lib-he-Microsoft.Maui.Controls.resources.dll.so => 22
	i64 u0x8c156fe7f184f137, ; 376: tr/Microsoft.Data.SqlClient.resources => 10
	i64 u0x8c1bafb2ed25af5b, ; 377: K4os.Compression.LZ4.Streams.dll => 55
	i64 u0x8c53ae18581b14f0, ; 378: Azure.Core => 48
	i64 u0x8cdfdb4ce85fb925, ; 379: lib_System.Security.Principal.Windows.dll.so => 202
	i64 u0x8d0f420977c2c1c7, ; 380: Xamarin.AndroidX.CursorAdapter.dll => 107
	i64 u0x8d7b8ab4b3310ead, ; 381: System.Threading => 212
	i64 u0x8da188285aadfe8e, ; 382: System.Collections.Concurrent => 134
	i64 u0x8e937db395a74375, ; 383: lib_Microsoft.Identity.Client.dll.so => 72
	i64 u0x8ec6e06a61c1baeb, ; 384: lib_Newtonsoft.Json.dll.so => 88
	i64 u0x8ed3cdd722b4d782, ; 385: System.Diagnostics.EventLog => 92
	i64 u0x8ed807bfe9858dfc, ; 386: Xamarin.AndroidX.Navigation.Common => 116
	i64 u0x8ee08b8194a30f48, ; 387: lib-hi-Microsoft.Maui.Controls.resources.dll.so => 23
	i64 u0x8ef7601039857a44, ; 388: lib-ro-Microsoft.Maui.Controls.resources.dll.so => 36
	i64 u0x8f32c6f611f6ffab, ; 389: pt/Microsoft.Maui.Controls.resources.dll => 35
	i64 u0x8f8829d21c8985a4, ; 390: lib-pt-BR-Microsoft.Maui.Controls.resources.dll.so => 34
	i64 u0x90263f8448b8f572, ; 391: lib_System.Diagnostics.TraceSource.dll.so => 150
	i64 u0x903101b46fb73a04, ; 392: _Microsoft.Android.Resource.Designer => 47
	i64 u0x90393bd4865292f3, ; 393: lib_System.IO.Compression.dll.so => 156
	i64 u0x905e2b8e7ae91ae6, ; 394: System.Threading.Tasks.Extensions.dll => 209
	i64 u0x90634f86c5ebe2b5, ; 395: Xamarin.AndroidX.Lifecycle.ViewModel.Android => 113
	i64 u0x907b636704ad79ef, ; 396: lib_Microsoft.Maui.Controls.Xaml.dll.so => 81
	i64 u0x91418dc638b29e68, ; 397: lib_Xamarin.AndroidX.CustomView.dll.so => 108
	i64 u0x9157bd523cd7ed36, ; 398: lib_System.Text.Json.dll.so => 206
	i64 u0x91a74f07b30d37e2, ; 399: System.Linq.dll => 163
	i64 u0x91fa41a87223399f, ; 400: ca/Microsoft.Maui.Controls.resources.dll => 14
	i64 u0x93489853b6098685, ; 401: es/Microsoft.Data.SqlClient.resources.dll => 2
	i64 u0x93cfa73ab28d6e35, ; 402: ms/Microsoft.Maui.Controls.resources => 30
	i64 u0x944077d8ca3c6580, ; 403: System.IO.Compression.dll => 156
	i64 u0x948d746a7702861f, ; 404: Microsoft.IdentityModel.Logging.dll => 76
	i64 u0x9502fd818eed2359, ; 405: lib_Microsoft.IdentityModel.Protocols.OpenIdConnect.dll.so => 78
	i64 u0x9564283c37ed59a9, ; 406: lib_Microsoft.IdentityModel.Logging.dll.so => 76
	i64 u0x965d480cfb8de46d, ; 407: pl/Microsoft.Data.SqlClient.resources.dll => 7
	i64 u0x967fc325e09bfa8c, ; 408: es/Microsoft.Maui.Controls.resources => 19
	i64 u0x96e49b31fe33d427, ; 409: Microsoft.Identity.Client.Extensions.Msal => 73
	i64 u0x9732d8dbddea3d9a, ; 410: id/Microsoft.Maui.Controls.resources => 26
	i64 u0x978be80e5210d31b, ; 411: Microsoft.Maui.Graphics.dll => 84
	i64 u0x97b8c771ea3e4220, ; 412: System.ComponentModel.dll => 143
	i64 u0x97e144c9d3c6976e, ; 413: System.Collections.Concurrent.dll => 134
	i64 u0x991d510397f92d9d, ; 414: System.Linq.Expressions => 161
	i64 u0x99868af5d93ecaeb, ; 415: lib_K4os.Hash.xxHash.dll.so => 56
	i64 u0x99a00ca5270c6878, ; 416: Xamarin.AndroidX.Navigation.Runtime => 118
	i64 u0x99cdc6d1f2d3a72f, ; 417: ko/Microsoft.Maui.Controls.resources.dll => 29
	i64 u0x9a0cc42c6f36dfc9, ; 418: lib_Microsoft.IdentityModel.Protocols.dll.so => 77
	i64 u0x9b211a749105beac, ; 419: System.Transactions.Local => 213
	i64 u0x9c244ac7cda32d26, ; 420: System.Security.Cryptography.X509Certificates.dll => 200
	i64 u0x9d5dbcf5a48583fe, ; 421: lib_Xamarin.AndroidX.Activity.dll.so => 99
	i64 u0x9d74dee1a7725f34, ; 422: Microsoft.Extensions.Configuration.Abstractions.dll => 65
	i64 u0x9e4534b6adaf6e84, ; 423: nl/Microsoft.Maui.Controls.resources => 32
	i64 u0x9eaf1efdf6f7267e, ; 424: Xamarin.AndroidX.Navigation.Common.dll => 116
	i64 u0x9ef542cf1f78c506, ; 425: Xamarin.AndroidX.Lifecycle.LiveData.Core => 112
	i64 u0x9ffbb6b1434ad2df, ; 426: Microsoft.Identity.Client.dll => 72
	i64 u0xa0d8259f4cc284ec, ; 427: lib_System.Security.Cryptography.dll.so => 201
	i64 u0xa1440773ee9d341e, ; 428: Xamarin.Google.Android.Material => 125
	i64 u0xa1b9d7c27f47219f, ; 429: Xamarin.AndroidX.Navigation.UI.dll => 119
	i64 u0xa2572680829d2c7c, ; 430: System.IO.Pipelines.dll => 159
	i64 u0xa3c64c49e90a9987, ; 431: System.Security.Cryptography.Pkcs => 95
	i64 u0xa46aa1eaa214539b, ; 432: ko/Microsoft.Maui.Controls.resources => 29
	i64 u0xa4edc8f2ceae241a, ; 433: System.Data.Common.dll => 145
	i64 u0xa5494f40f128ce6a, ; 434: System.Runtime.Serialization.Formatters.dll => 189
	i64 u0xa5b7152421ed6d98, ; 435: lib_System.IO.FileSystem.Watcher.dll.so => 158
	i64 u0xa5ce5c755bde8cb8, ; 436: lib_System.Security.Cryptography.Csp.dll.so => 197
	i64 u0xa5e599d1e0524750, ; 437: System.Numerics.Vectors.dll => 174
	i64 u0xa5f1ba49b85dd355, ; 438: System.Security.Cryptography.dll => 201
	i64 u0xa61975a5a37873ea, ; 439: lib_System.Xml.XmlSerializer.dll.so => 218
	i64 u0xa64476a892d76457, ; 440: lib_MySql.Data.dll.so => 86
	i64 u0xa67dbee13e1df9ca, ; 441: Xamarin.AndroidX.SavedState.dll => 121
	i64 u0xa68a420042bb9b1f, ; 442: Xamarin.AndroidX.DrawerLayout.dll => 109
	i64 u0xa71fe7d6f6f93efd, ; 443: Microsoft.Data.SqlClient => 58
	i64 u0xa763fbb98df8d9fb, ; 444: lib_Microsoft.Win32.Primitives.dll.so => 132
	i64 u0xa78ce3745383236a, ; 445: Xamarin.AndroidX.Lifecycle.Common.Jvm => 111
	i64 u0xa7c31b56b4dc7b33, ; 446: hu/Microsoft.Maui.Controls.resources => 25
	i64 u0xa8e6320dd07580ef, ; 447: lib_Microsoft.IdentityModel.JsonWebTokens.dll.so => 75
	i64 u0xa964304b5631e28a, ; 448: CommunityToolkit.Maui.Core.dll => 52
	i64 u0xaa2219c8e3449ff5, ; 449: Microsoft.Extensions.Logging.Abstractions => 69
	i64 u0xaa443ac34067eeef, ; 450: System.Private.Xml.dll => 179
	i64 u0xaa52de307ef5d1dd, ; 451: System.Net.Http => 165
	i64 u0xaa9a7b0214a5cc5c, ; 452: System.Diagnostics.StackTrace.dll => 148
	i64 u0xaaaf86367285a918, ; 453: Microsoft.Extensions.DependencyInjection.Abstractions.dll => 67
	i64 u0xaaf84bb3f052a265, ; 454: el/Microsoft.Maui.Controls.resources => 18
	i64 u0xab9c1b2687d86b0b, ; 455: lib_System.Linq.Expressions.dll.so => 161
	i64 u0xac2af3fa195a15ce, ; 456: System.Runtime.Numerics => 188
	i64 u0xac5376a2a538dc10, ; 457: Xamarin.AndroidX.Lifecycle.LiveData.Core.dll => 112
	i64 u0xac65e40f62b6b90e, ; 458: Google.Protobuf => 53
	i64 u0xac79c7e46047ad98, ; 459: System.Security.Principal.Windows.dll => 202
	i64 u0xac98d31068e24591, ; 460: System.Xml.XDocument => 217
	i64 u0xacd46e002c3ccb97, ; 461: ro/Microsoft.Maui.Controls.resources => 36
	i64 u0xacf42eea7ef9cd12, ; 462: System.Threading.Channels => 208
	i64 u0xad89c07347f1bad6, ; 463: nl/Microsoft.Maui.Controls.resources.dll => 32
	i64 u0xadbb53caf78a79d2, ; 464: System.Web.HttpUtility => 214
	i64 u0xadc90ab061a9e6e4, ; 465: System.ComponentModel.TypeConverter.dll => 142
	i64 u0xadf511667bef3595, ; 466: System.Net.Security => 170
	i64 u0xae0aaa94fdcfce0f, ; 467: System.ComponentModel.EventBasedAsync.dll => 140
	i64 u0xae282bcd03739de7, ; 468: Java.Interop => 222
	i64 u0xae53579c90db1107, ; 469: System.ObjectModel.dll => 175
	i64 u0xafe29f45095518e7, ; 470: lib_Xamarin.AndroidX.Lifecycle.ViewModelSavedState.dll.so => 114
	i64 u0xb05cc42cd94c6d9d, ; 471: lib-sv-Microsoft.Maui.Controls.resources.dll.so => 39
	i64 u0xb0bb43dc52ea59f9, ; 472: System.Diagnostics.Tracing.dll => 151
	i64 u0xb1dd05401aa8ee63, ; 473: System.Security.AccessControl => 194
	i64 u0xb220631954820169, ; 474: System.Text.RegularExpressions => 207
	i64 u0xb2376e1dbf8b4ed7, ; 475: System.Security.Cryptography.Csp => 197
	i64 u0xb2a3f67f3bf29fce, ; 476: da/Microsoft.Maui.Controls.resources => 16
	i64 u0xb398860d6ed7ba2f, ; 477: System.Security.Cryptography.ProtectedData => 96
	i64 u0xb3f0a0fcda8d3ebc, ; 478: Xamarin.AndroidX.CardView => 103
	i64 u0xb46be1aa6d4fff93, ; 479: hi/Microsoft.Maui.Controls.resources => 23
	i64 u0xb477491be13109d8, ; 480: ar/Microsoft.Maui.Controls.resources => 13
	i64 u0xb4bd7015ecee9d86, ; 481: System.IO.Pipelines => 159
	i64 u0xb4c53d9749c5f226, ; 482: lib_System.IO.FileSystem.AccessControl.dll.so => 157
	i64 u0xb5c7fcdafbc67ee4, ; 483: Microsoft.Extensions.Logging.Abstractions.dll => 69
	i64 u0xb5ea31d5244c6626, ; 484: System.Threading.ThreadPool.dll => 211
	i64 u0xb7212c4683a94afe, ; 485: System.Drawing.Primitives => 152
	i64 u0xb7b7753d1f319409, ; 486: sv/Microsoft.Maui.Controls.resources => 39
	i64 u0xb81a2c6e0aee50fe, ; 487: lib_System.Private.CoreLib.dll.so => 221
	i64 u0xb9185c33a1643eed, ; 488: Microsoft.CSharp.dll => 131
	i64 u0xb9f64d3b230def68, ; 489: lib-pt-Microsoft.Maui.Controls.resources.dll.so => 35
	i64 u0xb9fc3c8a556e3691, ; 490: ja/Microsoft.Maui.Controls.resources => 28
	i64 u0xba4670aa94a2b3c6, ; 491: lib_System.Xml.XDocument.dll.so => 217
	i64 u0xba48785529705af9, ; 492: System.Collections.dll => 138
	i64 u0xbadbc0a44214b54e, ; 493: K4os.Compression.LZ4 => 54
	i64 u0xbb65706fde942ce3, ; 494: System.Net.Sockets => 171
	i64 u0xbb8c8d165ef11460, ; 495: lib_Microsoft.Identity.Client.Extensions.Msal.dll.so => 73
	i64 u0xbbd180354b67271a, ; 496: System.Runtime.Serialization.Formatters => 189
	i64 u0xbcd22b365b764643, ; 497: lib-zh-Hans-Microsoft.Data.SqlClient.resources.dll.so => 11
	i64 u0xbd0aaf9dbfcc3376, ; 498: fr/Microsoft.Data.SqlClient.resources.dll => 3
	i64 u0xbd0e2c0d55246576, ; 499: System.Net.Http.dll => 165
	i64 u0xbd3c2d7a8325e11b, ; 500: lib-fr-Microsoft.Data.SqlClient.resources.dll.so => 3
	i64 u0xbd437a2cdb333d0d, ; 501: Xamarin.AndroidX.ViewPager2 => 124
	i64 u0xbd4aef17dbfb0390, ; 502: ru/Microsoft.Data.SqlClient.resources => 9
	i64 u0xbd5d0b88d3d647a5, ; 503: lib_Xamarin.AndroidX.Browser.dll.so => 102
	i64 u0xbd877b14d0b56392, ; 504: System.Runtime.Intrinsics.dll => 186
	i64 u0xbe65a49036345cf4, ; 505: lib_System.Buffers.dll.so => 133
	i64 u0xbee38d4a88835966, ; 506: Xamarin.AndroidX.AppCompat.AppCompatResources => 101
	i64 u0xc040a4ab55817f58, ; 507: ar/Microsoft.Maui.Controls.resources.dll => 13
	i64 u0xc0d928351ab5ca77, ; 508: System.Console.dll => 144
	i64 u0xc0f5a221a9383aea, ; 509: System.Runtime.Intrinsics => 186
	i64 u0xc12b8b3afa48329c, ; 510: lib_System.Linq.dll.so => 163
	i64 u0xc1c2cb7af77b8858, ; 511: Microsoft.EntityFrameworkCore => 59
	i64 u0xc1ff9ae3cdb6e1e6, ; 512: Xamarin.AndroidX.Activity.dll => 99
	i64 u0xc2260e1da1054ac1, ; 513: lib_BouncyCastle.Cryptography.dll.so => 50
	i64 u0xc26c064effb1dea9, ; 514: System.Buffers.dll => 133
	i64 u0xc278de356ad8a9e3, ; 515: Microsoft.IdentityModel.Logging => 76
	i64 u0xc28c50f32f81cc73, ; 516: ja/Microsoft.Maui.Controls.resources.dll => 28
	i64 u0xc2a3bca55b573141, ; 517: System.IO.FileSystem.Watcher => 158
	i64 u0xc2bcfec99f69365e, ; 518: Xamarin.AndroidX.ViewPager2.dll => 124
	i64 u0xc30b52815b58ac2c, ; 519: lib_System.Runtime.Serialization.Xml.dll.so => 192
	i64 u0xc463e077917aa21d, ; 520: System.Runtime.Serialization.Json => 190
	i64 u0xc472ce300460ccb6, ; 521: Microsoft.EntityFrameworkCore.dll => 59
	i64 u0xc4d3858ed4d08512, ; 522: Xamarin.AndroidX.Lifecycle.ViewModelSavedState.dll => 114
	i64 u0xc4d69851fe06342f, ; 523: lib_Microsoft.Extensions.Caching.Memory.dll.so => 63
	i64 u0xc50fded0ded1418c, ; 524: lib_System.ComponentModel.TypeConverter.dll.so => 142
	i64 u0xc519125d6bc8fb11, ; 525: lib_System.Net.Requests.dll.so => 169
	i64 u0xc5293b19e4dc230e, ; 526: Xamarin.AndroidX.Navigation.Fragment => 117
	i64 u0xc5325b2fcb37446f, ; 527: lib_System.Private.Xml.dll.so => 179
	i64 u0xc583d8477b5d3bac, ; 528: zh-Hant/Microsoft.Data.SqlClient.resources.dll => 12
	i64 u0xc5a0f4b95a699af7, ; 529: lib_System.Private.Uri.dll.so => 177
	i64 u0xc5cdcd5b6277579e, ; 530: lib_System.Security.Cryptography.Algorithms.dll.so => 196
	i64 u0xc6a4665a88c57225, ; 531: lib_ZstdSharp.dll.so => 129
	i64 u0xc7c01e7d7c93a110, ; 532: System.Text.Encoding.Extensions.dll => 204
	i64 u0xc7ce851898a4548e, ; 533: lib_System.Web.HttpUtility.dll.so => 214
	i64 u0xc858a28d9ee5a6c5, ; 534: lib_System.Collections.Specialized.dll.so => 137
	i64 u0xc9233ccffc57c512, ; 535: VatanSoft.VatanSms.Net.dll => 98
	i64 u0xc9c62c8f354ac568, ; 536: lib_System.Diagnostics.TextWriterTraceListener.dll.so => 149
	i64 u0xc9e54b32fc19baf3, ; 537: lib_CommunityToolkit.Maui.dll.so => 51
	i64 u0xca32340d8d54dcd5, ; 538: Microsoft.Extensions.Caching.Memory.dll => 63
	i64 u0xca3a723e7342c5b6, ; 539: lib-tr-Microsoft.Maui.Controls.resources.dll.so => 41
	i64 u0xcab3493c70141c2d, ; 540: pl/Microsoft.Maui.Controls.resources => 33
	i64 u0xcacfddc9f7c6de76, ; 541: ro/Microsoft.Maui.Controls.resources.dll => 36
	i64 u0xcb45618372c47127, ; 542: Microsoft.EntityFrameworkCore.Relational => 61
	i64 u0xcbd4fdd9cef4a294, ; 543: lib__Microsoft.Android.Resource.Designer.dll.so => 47
	i64 u0xcc182c3afdc374d6, ; 544: Microsoft.Bcl.AsyncInterfaces => 57
	i64 u0xcc2876b32ef2794c, ; 545: lib_System.Text.RegularExpressions.dll.so => 207
	i64 u0xcc5c3bb714c4561e, ; 546: Xamarin.KotlinX.Coroutines.Core.Jvm.dll => 127
	i64 u0xcc76886e09b88260, ; 547: Xamarin.KotlinX.Serialization.Core.Jvm.dll => 128
	i64 u0xccf25c4b634ccd3a, ; 548: zh-Hans/Microsoft.Maui.Controls.resources.dll => 45
	i64 u0xcd10a42808629144, ; 549: System.Net.Requests => 169
	i64 u0xcdd0c48b6937b21c, ; 550: Xamarin.AndroidX.SwipeRefreshLayout => 122
	i64 u0xceb28d385f84f441, ; 551: Azure.Core.dll => 48
	i64 u0xcf140ed700bc8e66, ; 552: Microsoft.SqlServer.Server.dll => 85
	i64 u0xcf23d8093f3ceadf, ; 553: System.Diagnostics.DiagnosticSource.dll => 146
	i64 u0xcf8fc898f98b0d34, ; 554: System.Private.Xml.Linq => 178
	i64 u0xd063299fcfc0c93f, ; 555: lib_System.Runtime.Serialization.Json.dll.so => 190
	i64 u0xd0de8a113e976700, ; 556: System.Diagnostics.TextWriterTraceListener => 149
	i64 u0xd1194e1d8a8de83c, ; 557: lib_Xamarin.AndroidX.Lifecycle.Common.Jvm.dll.so => 111
	i64 u0xd22a0c4630f2fe66, ; 558: lib_System.Security.Cryptography.ProtectedData.dll.so => 96
	i64 u0xd2dffb59201927bd, ; 559: de/Microsoft.Data.SqlClient.resources.dll => 1
	i64 u0xd333d0af9e423810, ; 560: System.Runtime.InteropServices => 185
	i64 u0xd33a415cb4278969, ; 561: System.Security.Cryptography.Encoding.dll => 198
	i64 u0xd3426d966bb704f5, ; 562: Xamarin.AndroidX.AppCompat.AppCompatResources.dll => 101
	i64 u0xd3651b6fc3125825, ; 563: System.Private.Uri.dll => 177
	i64 u0xd373685349b1fe8b, ; 564: Microsoft.Extensions.Logging.dll => 68
	i64 u0xd3801faafafb7698, ; 565: System.Private.DataContractSerialization.dll => 176
	i64 u0xd3e4c8d6a2d5d470, ; 566: it/Microsoft.Maui.Controls.resources => 27
	i64 u0xd42655883bb8c19f, ; 567: Microsoft.EntityFrameworkCore.Abstractions.dll => 60
	i64 u0xd4645626dffec99d, ; 568: lib_Microsoft.Extensions.DependencyInjection.Abstractions.dll.so => 67
	i64 u0xd5507e11a2b2839f, ; 569: Xamarin.AndroidX.Lifecycle.ViewModelSavedState => 114
	i64 u0xd5858610826f1c08, ; 570: lib-ru-Microsoft.Data.SqlClient.resources.dll.so => 9
	i64 u0xd6694f8359737e4e, ; 571: Xamarin.AndroidX.SavedState => 121
	i64 u0xd6d21782156bc35b, ; 572: Xamarin.AndroidX.SwipeRefreshLayout.dll => 122
	i64 u0xd72329819cbbbc44, ; 573: lib_Microsoft.Extensions.Configuration.Abstractions.dll.so => 65
	i64 u0xd72c760af136e863, ; 574: System.Xml.XmlSerializer.dll => 218
	i64 u0xd7b3764ada9d341d, ; 575: lib_Microsoft.Extensions.Logging.Abstractions.dll.so => 69
	i64 u0xd9e245a1762ddad5, ; 576: BouncyCastle.Cryptography => 50
	i64 u0xda1dfa4c534a9251, ; 577: Microsoft.Extensions.DependencyInjection => 66
	i64 u0xdad05a11827959a3, ; 578: System.Collections.NonGeneric.dll => 136
	i64 u0xdb5383ab5865c007, ; 579: lib-vi-Microsoft.Maui.Controls.resources.dll.so => 43
	i64 u0xdb58816721c02a59, ; 580: lib_System.Reflection.Emit.ILGeneration.dll.so => 180
	i64 u0xdbeda89f832aa805, ; 581: vi/Microsoft.Maui.Controls.resources.dll => 43
	i64 u0xdbf2a779fbc3ac31, ; 582: System.Transactions.Local.dll => 213
	i64 u0xdbf9607a441b4505, ; 583: System.Linq => 163
	i64 u0xdc75032002d1a212, ; 584: lib_System.Transactions.Local.dll.so => 213
	i64 u0xdca8be7403f92d4f, ; 585: lib_System.Linq.Queryable.dll.so => 162
	i64 u0xdce2c53525640bf3, ; 586: Microsoft.Extensions.Logging => 68
	i64 u0xdd2b722d78ef5f43, ; 587: System.Runtime.dll => 193
	i64 u0xdd67031857c72f96, ; 588: lib_System.Text.Encodings.Web.dll.so => 205
	i64 u0xdde30e6b77aa6f6c, ; 589: lib-zh-Hans-Microsoft.Maui.Controls.resources.dll.so => 45
	i64 u0xde110ae80fa7c2e2, ; 590: System.Xml.XDocument.dll => 217
	i64 u0xde572c2b2fb32f93, ; 591: lib_System.Threading.Tasks.Extensions.dll.so => 209
	i64 u0xde8769ebda7d8647, ; 592: hr/Microsoft.Maui.Controls.resources.dll => 24
	i64 u0xdf35b6d818902893, ; 593: K4os.Hash.xxHash.dll => 56
	i64 u0xe0142572c095a480, ; 594: Xamarin.AndroidX.AppCompat.dll => 100
	i64 u0xe02f89350ec78051, ; 595: Xamarin.AndroidX.CoordinatorLayout.dll => 105
	i64 u0xe0ea30f1ac5b7731, ; 596: ko/Microsoft.Data.SqlClient.resources.dll => 6
	i64 u0xe0ee2e61123c1478, ; 597: lib-es-Microsoft.Data.SqlClient.resources.dll.so => 2
	i64 u0xe10b760bb1462e7a, ; 598: lib_System.Security.Cryptography.Primitives.dll.so => 199
	i64 u0xe12265280d0b036d, ; 599: fr/Microsoft.Data.SqlClient.resources => 3
	i64 u0xe192a588d4410686, ; 600: lib_System.IO.Pipelines.dll.so => 159
	i64 u0xe1a08bd3fa539e0d, ; 601: System.Runtime.Loader => 187
	i64 u0xe1b52f9f816c70ef, ; 602: System.Private.Xml.Linq.dll => 178
	i64 u0xe1ecfdb7fff86067, ; 603: System.Net.Security.dll => 170
	i64 u0xe22fa4c9c645db62, ; 604: System.Diagnostics.TextWriterTraceListener.dll => 149
	i64 u0xe2420585aeceb728, ; 605: System.Net.Requests.dll => 169
	i64 u0xe29b73bc11392966, ; 606: lib-id-Microsoft.Maui.Controls.resources.dll.so => 26
	i64 u0xe2e426c7714fa0bc, ; 607: Microsoft.Win32.Primitives.dll => 132
	i64 u0xe3811d68d4fe8463, ; 608: pt-BR/Microsoft.Maui.Controls.resources.dll => 34
	i64 u0xe3b7cbae5ad66c75, ; 609: lib_System.Security.Cryptography.Encoding.dll.so => 198
	i64 u0xe494f7ced4ecd10a, ; 610: hu/Microsoft.Maui.Controls.resources.dll => 25
	i64 u0xe4a9b1e40d1e8917, ; 611: lib-fi-Microsoft.Maui.Controls.resources.dll.so => 20
	i64 u0xe4f74a0b5bf9703f, ; 612: System.Runtime.Serialization.Primitives => 191
	i64 u0xe5434e8a119ceb69, ; 613: lib_Mono.Android.dll.so => 224
	i64 u0xe57d22ca4aeb4900, ; 614: System.Configuration.ConfigurationManager => 90
	i64 u0xe7e03cc18dcdeb49, ; 615: lib_System.Diagnostics.StackTrace.dll.so => 148
	i64 u0xe89a2a9ef110899b, ; 616: System.Drawing.dll => 153
	i64 u0xe95d3096c1827a3f, ; 617: lib_MySql.EntityFrameworkCore.dll.so => 87
	i64 u0xed6ef763c6fb395f, ; 618: System.Diagnostics.EventLog.dll => 92
	i64 u0xedc4817167106c23, ; 619: System.Net.Sockets.dll => 171
	i64 u0xedc632067fb20ff3, ; 620: System.Memory.dll => 164
	i64 u0xedc8e4ca71a02a8b, ; 621: Xamarin.AndroidX.Navigation.Runtime.dll => 118
	i64 u0xee81f5b3f1c4f83b, ; 622: System.Threading.ThreadPool => 211
	i64 u0xeeb7ebb80150501b, ; 623: lib_Xamarin.AndroidX.Collection.Jvm.dll.so => 104
	i64 u0xef03b1b5a04e9709, ; 624: System.Text.Encoding.CodePages.dll => 203
	i64 u0xef72742e1bcca27a, ; 625: Microsoft.Maui.Essentials.dll => 83
	i64 u0xefd0396433f04886, ; 626: pt-BR/Microsoft.Data.SqlClient.resources => 8
	i64 u0xefec0b7fdc57ec42, ; 627: Xamarin.AndroidX.Activity => 99
	i64 u0xf00c29406ea45e19, ; 628: es/Microsoft.Maui.Controls.resources.dll => 19
	i64 u0xf09e47b6ae914f6e, ; 629: System.Net.NameResolution => 166
	i64 u0xf0de2537ee19c6ca, ; 630: lib_System.Net.WebHeaderCollection.dll.so => 173
	i64 u0xf11b621fc87b983f, ; 631: Microsoft.Maui.Controls.Xaml.dll => 81
	i64 u0xf1c4b4005493d871, ; 632: System.Formats.Asn1.dll => 154
	i64 u0xf238bd79489d3a96, ; 633: lib-nl-Microsoft.Maui.Controls.resources.dll.so => 32
	i64 u0xf37221fda4ef8830, ; 634: lib_Xamarin.Google.Android.Material.dll.so => 125
	i64 u0xf3ddfe05336abf29, ; 635: System => 219
	i64 u0xf408654b2a135055, ; 636: System.Reflection.Emit.ILGeneration.dll => 180
	i64 u0xf4103170a1de5bd0, ; 637: System.Linq.Queryable.dll => 162
	i64 u0xf4c1dd70a5496a17, ; 638: System.IO.Compression => 156
	i64 u0xf5e59d7ac34b50aa, ; 639: Microsoft.IdentityModel.Protocols.dll => 77
	i64 u0xf5fc7602fe27b333, ; 640: System.Net.WebHeaderCollection => 173
	i64 u0xf6077741019d7428, ; 641: Xamarin.AndroidX.CoordinatorLayout => 105
	i64 u0xf61ade9836ad4692, ; 642: Microsoft.IdentityModel.Tokens.dll => 79
	i64 u0xf6c0e7d55a7a4e4f, ; 643: Microsoft.IdentityModel.JsonWebTokens => 75
	i64 u0xf77b20923f07c667, ; 644: de/Microsoft.Maui.Controls.resources.dll => 17
	i64 u0xf7be8a85d06b4b64, ; 645: ru/Microsoft.Data.SqlClient.resources.dll => 9
	i64 u0xf7e2cac4c45067b3, ; 646: lib_System.Numerics.Vectors.dll.so => 174
	i64 u0xf7e74930e0e3d214, ; 647: zh-HK/Microsoft.Maui.Controls.resources.dll => 44
	i64 u0xf7fa0bf77fe677cc, ; 648: Newtonsoft.Json.dll => 88
	i64 u0xf83775f330791063, ; 649: ja/Microsoft.Data.SqlClient.resources.dll => 5
	i64 u0xf84773b5c81e3cef, ; 650: lib-uk-Microsoft.Maui.Controls.resources.dll.so => 42
	i64 u0xf8aac5ea82de1348, ; 651: System.Linq.Queryable => 162
	i64 u0xf8b77539b362d3ba, ; 652: lib_System.Reflection.Primitives.dll.so => 182
	i64 u0xf8cd217ba1bbfdc8, ; 653: lib-zh-Hant-Microsoft.Data.SqlClient.resources.dll.so => 12
	i64 u0xf8e045dc345b2ea3, ; 654: lib_Xamarin.AndroidX.RecyclerView.dll.so => 120
	i64 u0xf915dc29808193a1, ; 655: System.Web.HttpUtility.dll => 214
	i64 u0xf96c777a2a0686f4, ; 656: hi/Microsoft.Maui.Controls.resources.dll => 23
	i64 u0xf9be54c8bcf8ff3b, ; 657: System.Security.AccessControl.dll => 194
	i64 u0xf9eec5bb3a6aedc6, ; 658: Microsoft.Extensions.Options => 70
	i64 u0xfa2fdb27e8a2c8e8, ; 659: System.ComponentModel.EventBasedAsync => 140
	i64 u0xfa3f278f288b0e84, ; 660: lib_System.Net.Security.dll.so => 170
	i64 u0xfa5ed7226d978949, ; 661: lib-ar-Microsoft.Maui.Controls.resources.dll.so => 13
	i64 u0xfa645d91e9fc4cba, ; 662: System.Threading.Thread => 210
	i64 u0xfbad3e4ce4b98145, ; 663: System.Security.Cryptography.X509Certificates => 200
	i64 u0xfbf0a31c9fc34bc4, ; 664: lib_System.Net.Http.dll.so => 165
	i64 u0xfc6b7527cc280b3f, ; 665: lib_System.Runtime.Serialization.Formatters.dll.so => 189
	i64 u0xfc719aec26adf9d9, ; 666: Xamarin.AndroidX.Navigation.Fragment.dll => 117
	i64 u0xfcd5b90cf101e36b, ; 667: System.Data.SqlClient.dll => 91
	i64 u0xfd22f00870e40ae0, ; 668: lib_Xamarin.AndroidX.DrawerLayout.dll.so => 109
	i64 u0xfd49b3c1a76e2748, ; 669: System.Runtime.InteropServices.RuntimeInformation => 184
	i64 u0xfd536c702f64dc47, ; 670: System.Text.Encoding.Extensions => 204
	i64 u0xfd583f7657b6a1cb, ; 671: Xamarin.AndroidX.Fragment => 110
	i64 u0xfdbe4710aa9beeff, ; 672: CommunityToolkit.Maui => 51
	i64 u0xfeae9952cf03b8cb, ; 673: tr/Microsoft.Maui.Controls.resources => 41
	i64 u0xfff40914e0b38d3d ; 674: Azure.Identity.dll => 49
], align 8

@assembly_image_cache_indices = dso_local local_unnamed_addr constant [675 x i32] [
	i32 1, i32 122, i32 139, i32 118, i32 52, i32 95, i32 6, i32 63,
	i32 223, i32 100, i32 133, i32 176, i32 37, i32 15, i32 43, i32 74,
	i32 168, i32 120, i32 138, i32 82, i32 183, i32 12, i32 10, i32 44,
	i32 215, i32 104, i32 37, i32 136, i32 4, i32 48, i32 182, i32 109,
	i32 139, i32 70, i32 136, i32 94, i32 201, i32 62, i32 208, i32 58,
	i32 38, i32 128, i32 123, i32 34, i32 224, i32 83, i32 57, i32 166,
	i32 5, i32 57, i32 108, i32 2, i32 155, i32 199, i32 181, i32 54,
	i32 120, i32 78, i32 102, i32 21, i32 222, i32 22, i32 78, i32 67,
	i32 132, i32 92, i32 1, i32 160, i32 196, i32 220, i32 194, i32 25,
	i32 205, i32 128, i32 172, i32 31, i32 195, i32 49, i32 134, i32 219,
	i32 40, i32 223, i32 157, i32 53, i32 148, i32 119, i32 29, i32 70,
	i32 218, i32 172, i32 155, i32 97, i32 147, i32 193, i32 89, i32 180,
	i32 40, i32 160, i32 54, i32 210, i32 59, i32 130, i32 0, i32 144,
	i32 106, i32 191, i32 21, i32 126, i32 71, i32 93, i32 26, i32 24,
	i32 222, i32 168, i32 130, i32 42, i32 167, i32 150, i32 20, i32 207,
	i32 93, i32 154, i32 46, i32 33, i32 203, i32 181, i32 178, i32 8,
	i32 212, i32 39, i32 206, i32 18, i32 147, i32 216, i32 85, i32 151,
	i32 62, i32 110, i32 74, i32 47, i32 87, i32 103, i32 152, i32 21,
	i32 216, i32 135, i32 19, i32 171, i32 85, i32 82, i32 15, i32 80,
	i32 139, i32 124, i32 64, i32 186, i32 181, i32 182, i32 135, i32 183,
	i32 108, i32 166, i32 77, i32 90, i32 123, i32 14, i32 88, i32 199,
	i32 86, i32 204, i32 79, i32 200, i32 58, i32 126, i32 102, i32 91,
	i32 211, i32 215, i32 106, i32 53, i32 4, i32 73, i32 209, i32 86,
	i32 116, i32 49, i32 90, i32 101, i32 220, i32 224, i32 33, i32 131,
	i32 191, i32 126, i32 60, i32 93, i32 150, i32 37, i32 215, i32 94,
	i32 35, i32 97, i32 175, i32 119, i32 157, i32 206, i32 89, i32 61,
	i32 115, i32 167, i32 161, i32 179, i32 89, i32 187, i32 27, i32 115,
	i32 223, i32 203, i32 208, i32 14, i32 0, i32 79, i32 4, i32 80,
	i32 113, i32 5, i32 153, i32 168, i32 145, i32 11, i32 106, i32 84,
	i32 38, i32 167, i32 184, i32 44, i32 195, i32 193, i32 111, i32 137,
	i32 192, i32 177, i32 221, i32 146, i32 28, i32 66, i32 131, i32 94,
	i32 105, i32 212, i32 143, i32 176, i32 16, i32 56, i32 91, i32 68,
	i32 173, i32 8, i32 7, i32 185, i32 104, i32 137, i32 205, i32 141,
	i32 197, i32 216, i32 145, i32 18, i32 183, i32 66, i32 127, i32 164,
	i32 55, i32 81, i32 17, i32 187, i32 0, i32 221, i32 10, i32 135,
	i32 125, i32 196, i32 51, i32 129, i32 80, i32 188, i32 144, i32 113,
	i32 107, i32 16, i32 152, i32 154, i32 22, i32 185, i32 31, i32 87,
	i32 96, i32 84, i32 7, i32 140, i32 71, i32 107, i32 71, i32 117,
	i32 82, i32 15, i32 158, i32 41, i32 31, i32 27, i32 141, i32 95,
	i32 198, i32 24, i32 164, i32 64, i32 98, i32 121, i32 188, i32 55,
	i32 30, i32 40, i32 97, i32 130, i32 110, i32 11, i32 20, i32 75,
	i32 142, i32 38, i32 17, i32 172, i32 52, i32 30, i32 174, i32 138,
	i32 50, i32 151, i32 195, i32 175, i32 143, i32 123, i32 65, i32 72,
	i32 112, i32 219, i32 46, i32 129, i32 100, i32 103, i32 153, i32 42,
	i32 98, i32 74, i32 45, i32 160, i32 190, i32 61, i32 6, i32 46,
	i32 64, i32 192, i32 60, i32 210, i32 155, i32 83, i32 127, i32 220,
	i32 141, i32 184, i32 62, i32 115, i32 146, i32 202, i32 147, i32 22,
	i32 10, i32 55, i32 48, i32 202, i32 107, i32 212, i32 134, i32 72,
	i32 88, i32 92, i32 116, i32 23, i32 36, i32 35, i32 34, i32 150,
	i32 47, i32 156, i32 209, i32 113, i32 81, i32 108, i32 206, i32 163,
	i32 14, i32 2, i32 30, i32 156, i32 76, i32 78, i32 76, i32 7,
	i32 19, i32 73, i32 26, i32 84, i32 143, i32 134, i32 161, i32 56,
	i32 118, i32 29, i32 77, i32 213, i32 200, i32 99, i32 65, i32 32,
	i32 116, i32 112, i32 72, i32 201, i32 125, i32 119, i32 159, i32 95,
	i32 29, i32 145, i32 189, i32 158, i32 197, i32 174, i32 201, i32 218,
	i32 86, i32 121, i32 109, i32 58, i32 132, i32 111, i32 25, i32 75,
	i32 52, i32 69, i32 179, i32 165, i32 148, i32 67, i32 18, i32 161,
	i32 188, i32 112, i32 53, i32 202, i32 217, i32 36, i32 208, i32 32,
	i32 214, i32 142, i32 170, i32 140, i32 222, i32 175, i32 114, i32 39,
	i32 151, i32 194, i32 207, i32 197, i32 16, i32 96, i32 103, i32 23,
	i32 13, i32 159, i32 157, i32 69, i32 211, i32 152, i32 39, i32 221,
	i32 131, i32 35, i32 28, i32 217, i32 138, i32 54, i32 171, i32 73,
	i32 189, i32 11, i32 3, i32 165, i32 3, i32 124, i32 9, i32 102,
	i32 186, i32 133, i32 101, i32 13, i32 144, i32 186, i32 163, i32 59,
	i32 99, i32 50, i32 133, i32 76, i32 28, i32 158, i32 124, i32 192,
	i32 190, i32 59, i32 114, i32 63, i32 142, i32 169, i32 117, i32 179,
	i32 12, i32 177, i32 196, i32 129, i32 204, i32 214, i32 137, i32 98,
	i32 149, i32 51, i32 63, i32 41, i32 33, i32 36, i32 61, i32 47,
	i32 57, i32 207, i32 127, i32 128, i32 45, i32 169, i32 122, i32 48,
	i32 85, i32 146, i32 178, i32 190, i32 149, i32 111, i32 96, i32 1,
	i32 185, i32 198, i32 101, i32 177, i32 68, i32 176, i32 27, i32 60,
	i32 67, i32 114, i32 9, i32 121, i32 122, i32 65, i32 218, i32 69,
	i32 50, i32 66, i32 136, i32 43, i32 180, i32 43, i32 213, i32 163,
	i32 213, i32 162, i32 68, i32 193, i32 205, i32 45, i32 217, i32 209,
	i32 24, i32 56, i32 100, i32 105, i32 6, i32 2, i32 199, i32 3,
	i32 159, i32 187, i32 178, i32 170, i32 149, i32 169, i32 26, i32 132,
	i32 34, i32 198, i32 25, i32 20, i32 191, i32 224, i32 90, i32 148,
	i32 153, i32 87, i32 92, i32 171, i32 164, i32 118, i32 211, i32 104,
	i32 203, i32 83, i32 8, i32 99, i32 19, i32 166, i32 173, i32 81,
	i32 154, i32 32, i32 125, i32 219, i32 180, i32 162, i32 156, i32 77,
	i32 173, i32 105, i32 79, i32 75, i32 17, i32 9, i32 174, i32 44,
	i32 88, i32 5, i32 42, i32 162, i32 182, i32 12, i32 120, i32 214,
	i32 23, i32 194, i32 70, i32 140, i32 170, i32 13, i32 210, i32 200,
	i32 165, i32 189, i32 117, i32 91, i32 109, i32 184, i32 204, i32 110,
	i32 51, i32 41, i32 49
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
!2 = !{!".NET for Android remotes/origin/release/9.0.1xx @ e7876a4f92d894b40c191a24c2b74f06d4bf4573"}
!3 = !{!4, !4, i64 0}
!4 = !{!"any pointer", !5, i64 0}
!5 = !{!"omnipotent char", !6, i64 0}
!6 = !{!"Simple C++ TBAA"}
!7 = !{i32 1, !"branch-target-enforcement", i32 0}
!8 = !{i32 1, !"sign-return-address", i32 0}
!9 = !{i32 1, !"sign-return-address-all", i32 0}
!10 = !{i32 1, !"sign-return-address-with-bkey", i32 0}
