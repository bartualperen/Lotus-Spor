; ModuleID = 'marshal_methods.x86_64.ll'
source_filename = "marshal_methods.x86_64.ll"
target datalayout = "e-m:e-p270:32:32-p271:32:32-p272:64:64-i64:64-f80:128-n8:16:32:64-S128"
target triple = "x86_64-unknown-linux-android21"

%struct.MarshalMethodName = type {
	i64, ; uint64_t id
	ptr ; char* name
}

%struct.MarshalMethodsManagedClass = type {
	i32, ; uint32_t token
	ptr ; MonoClass klass
}

@assembly_image_cache = dso_local local_unnamed_addr global [223 x ptr] zeroinitializer, align 16

; Each entry maps hash of an assembly name to an index into the `assembly_image_cache` array
@assembly_image_cache_hashes = dso_local local_unnamed_addr constant [669 x i64] [
	i64 u0x006b9d7c1c7e1c42, ; 0: de/Microsoft.Data.SqlClient.resources => 0
	i64 u0x0071cf2d27b7d61e, ; 1: lib_Xamarin.AndroidX.SwipeRefreshLayout.dll.so => 117
	i64 u0x01109b0e4d99e61f, ; 2: System.ComponentModel.Annotations.dll => 134
	i64 u0x02123411c4e01926, ; 3: lib_Xamarin.AndroidX.Navigation.Runtime.dll.so => 113
	i64 u0x029b2c18aaa0996c, ; 4: lib-ko-Microsoft.Data.SqlClient.resources.dll.so => 5
	i64 u0x02a4c5a44384f885, ; 5: Microsoft.Extensions.Caching.Memory => 59
	i64 u0x02abedc11addc1ed, ; 6: lib_Mono.Android.Runtime.dll.so => 221
	i64 u0x032267b2a94db371, ; 7: lib_Xamarin.AndroidX.AppCompat.dll.so => 95
	i64 u0x03621c804933a890, ; 8: System.Buffers => 128
	i64 u0x0399610510a38a38, ; 9: lib_System.Private.DataContractSerialization.dll.so => 172
	i64 u0x043032f1d071fae0, ; 10: ru/Microsoft.Maui.Controls.resources => 34
	i64 u0x044440a55165631e, ; 11: lib-cs-Microsoft.Maui.Controls.resources.dll.so => 12
	i64 u0x046eb1581a80c6b0, ; 12: vi/Microsoft.Maui.Controls.resources => 40
	i64 u0x0470607fd33c32db, ; 13: Microsoft.IdentityModel.Abstractions.dll => 70
	i64 u0x0517ef04e06e9f76, ; 14: System.Net.Primitives => 164
	i64 u0x0565d18c6da3de38, ; 15: Xamarin.AndroidX.RecyclerView => 115
	i64 u0x0581db89237110e9, ; 16: lib_System.Collections.dll.so => 133
	i64 u0x05989cb940b225a9, ; 17: Microsoft.Maui.dll => 78
	i64 u0x05a1c25e78e22d87, ; 18: lib_System.Runtime.CompilerServices.Unsafe.dll.so => 180
	i64 u0x05d8ca8ee551619f, ; 19: zh-Hant/Microsoft.Data.SqlClient.resources => 9
	i64 u0x06076b5d2b581f08, ; 20: zh-HK/Microsoft.Maui.Controls.resources => 41
	i64 u0x06388ffe9f6c161a, ; 21: System.Xml.Linq.dll => 213
	i64 u0x0680a433c781bb3d, ; 22: Xamarin.AndroidX.Collection.Jvm => 99
	i64 u0x07bae9a59f101b46, ; 23: Microsoft.Bcl.HashCode.dll => 53
	i64 u0x07c57877c7ba78ad, ; 24: ru/Microsoft.Maui.Controls.resources.dll => 34
	i64 u0x07dcdc7460a0c5e4, ; 25: System.Collections.NonGeneric => 131
	i64 u0x08015600dcbf6dc7, ; 26: it/Microsoft.Data.SqlClient.resources.dll => 3
	i64 u0x08881a0a9768df86, ; 27: lib_Azure.Core.dll.so => 45
	i64 u0x08a7c865576bbde7, ; 28: System.Reflection.Primitives => 178
	i64 u0x08f3c9788ee2153c, ; 29: Xamarin.AndroidX.DrawerLayout => 104
	i64 u0x09138715c92dba90, ; 30: lib_System.ComponentModel.Annotations.dll.so => 134
	i64 u0x0919c28b89381a0b, ; 31: lib_Microsoft.Extensions.Options.dll.so => 66
	i64 u0x092266563089ae3e, ; 32: lib_System.Collections.NonGeneric.dll.so => 131
	i64 u0x095cacaf6b6a32e4, ; 33: System.Memory.Data => 89
	i64 u0x09d144a7e214d457, ; 34: System.Security.Cryptography => 199
	i64 u0x0a805f95d98f597b, ; 35: lib_Microsoft.Extensions.Caching.Abstractions.dll.so => 58
	i64 u0x0abb3e2b271edc45, ; 36: System.Threading.Channels.dll => 205
	i64 u0x0adeb6c0f5699d33, ; 37: Microsoft.Data.SqlClient.dll => 54
	i64 u0x0b3b632c3bbee20c, ; 38: sk/Microsoft.Maui.Controls.resources => 35
	i64 u0x0b6aff547b84fbe9, ; 39: Xamarin.KotlinX.Serialization.Core.Jvm => 123
	i64 u0x0be2e1f8ce4064ed, ; 40: Xamarin.AndroidX.ViewPager => 118
	i64 u0x0c3ca6cc978e2aae, ; 41: pt-BR/Microsoft.Maui.Controls.resources => 31
	i64 u0x0c59ad9fbbd43abe, ; 42: Mono.Android => 222
	i64 u0x0c7790f60165fc06, ; 43: lib_Microsoft.Maui.Essentials.dll.so => 79
	i64 u0x0d3b5ab8b2766190, ; 44: lib_Microsoft.Bcl.AsyncInterfaces.dll.so => 52
	i64 u0x0e14e73a54dda68e, ; 45: lib_System.Net.NameResolution.dll.so => 162
	i64 u0x0fbe06392ef90569, ; 46: lib-ja-Microsoft.Data.SqlClient.resources.dll.so => 4
	i64 u0x102861e4055f511a, ; 47: Microsoft.Bcl.AsyncInterfaces.dll => 52
	i64 u0x102a31b45304b1da, ; 48: Xamarin.AndroidX.CustomView => 103
	i64 u0x108cf0e0ba098a51, ; 49: es/Microsoft.Data.SqlClient.resources => 1
	i64 u0x10f6cfcbcf801616, ; 50: System.IO.Compression.Brotli => 151
	i64 u0x114443cdcf2091f1, ; 51: System.Security.Cryptography.Primitives => 197
	i64 u0x123639456fb056da, ; 52: System.Reflection.Emit.Lightweight.dll => 177
	i64 u0x124f38a5d8cb5fb8, ; 53: K4os.Compression.LZ4.dll => 49
	i64 u0x125b7f94acb989db, ; 54: Xamarin.AndroidX.RecyclerView.dll => 115
	i64 u0x126ee4b0de53cbfd, ; 55: Microsoft.IdentityModel.Protocols.OpenIdConnect.dll => 74
	i64 u0x138567fa954faa55, ; 56: Xamarin.AndroidX.Browser => 97
	i64 u0x13a01de0cbc3f06c, ; 57: lib-fr-Microsoft.Maui.Controls.resources.dll.so => 18
	i64 u0x13f1e5e209e91af4, ; 58: lib_Java.Interop.dll.so => 220
	i64 u0x13f1e880c25d96d1, ; 59: he/Microsoft.Maui.Controls.resources => 19
	i64 u0x143a1f6e62b82b56, ; 60: Microsoft.IdentityModel.Protocols.OpenIdConnect => 74
	i64 u0x143d8ea60a6a4011, ; 61: Microsoft.Extensions.DependencyInjection.Abstractions => 63
	i64 u0x152a448bd1e745a7, ; 62: Microsoft.Win32.Primitives => 127
	i64 u0x159cc6c81072f00e, ; 63: lib_System.Diagnostics.EventLog.dll.so => 87
	i64 u0x162be8a76b00cd97, ; 64: lib-de-Microsoft.Data.SqlClient.resources.dll.so => 0
	i64 u0x16bf2a22df043a09, ; 65: System.IO.Pipes.dll => 156
	i64 u0x16ea2b318ad2d830, ; 66: System.Security.Cryptography.Algorithms => 193
	i64 u0x17125c9a85b4929f, ; 67: lib_netstandard.dll.so => 218
	i64 u0x1716866f7416792e, ; 68: lib_System.Security.AccessControl.dll.so => 191
	i64 u0x17b56e25558a5d36, ; 69: lib-hu-Microsoft.Maui.Controls.resources.dll.so => 22
	i64 u0x17f9358913beb16a, ; 70: System.Text.Encodings.Web => 203
	i64 u0x18402a709e357f3b, ; 71: lib_Xamarin.KotlinX.Serialization.Core.Jvm.dll.so => 123
	i64 u0x18a9befae51bb361, ; 72: System.Net.WebClient => 168
	i64 u0x18f0ce884e87d89a, ; 73: nb/Microsoft.Maui.Controls.resources.dll => 28
	i64 u0x19a4c090f14ebb66, ; 74: System.Security.Claims => 192
	i64 u0x1a6fceea64859810, ; 75: Azure.Identity => 46
	i64 u0x1a91866a319e9259, ; 76: lib_System.Collections.Concurrent.dll.so => 129
	i64 u0x1aac34d1917ba5d3, ; 77: lib_System.dll.so => 217
	i64 u0x1aad60783ffa3e5b, ; 78: lib-th-Microsoft.Maui.Controls.resources.dll.so => 37
	i64 u0x1c753b5ff15bce1b, ; 79: Mono.Android.Runtime.dll => 221
	i64 u0x1cd47467799d8250, ; 80: System.Threading.Tasks.dll => 207
	i64 u0x1db6820994506bf5, ; 81: System.IO.FileSystem.AccessControl.dll => 153
	i64 u0x1dba6509cc55b56f, ; 82: lib_Google.Protobuf.dll.so => 48
	i64 u0x1dbb0c2c6a999acb, ; 83: System.Diagnostics.StackTrace => 144
	i64 u0x1e3d87657e9659bc, ; 84: Xamarin.AndroidX.Navigation.UI => 114
	i64 u0x1e71143913d56c10, ; 85: lib-ko-Microsoft.Maui.Controls.resources.dll.so => 26
	i64 u0x1ed8fcce5e9b50a0, ; 86: Microsoft.Extensions.Options.dll => 66
	i64 u0x1f055d15d807e1b2, ; 87: System.Xml.XmlSerializer => 216
	i64 u0x20237ea48006d7a8, ; 88: lib_System.Net.WebClient.dll.so => 168
	i64 u0x209375905fcc1bad, ; 89: lib_System.IO.Compression.Brotli.dll.so => 151
	i64 u0x20edad43b59fbd8e, ; 90: System.Security.Permissions.dll => 92
	i64 u0x20fab3cf2dfbc8df, ; 91: lib_System.Diagnostics.Process.dll.so => 143
	i64 u0x2174319c0d835bc9, ; 92: System.Runtime => 190
	i64 u0x2199f06354c82d3b, ; 93: System.ClientModel.dll => 84
	i64 u0x21cc7e445dcd5469, ; 94: System.Reflection.Emit.ILGeneration => 176
	i64 u0x220fd4f2e7c48170, ; 95: th/Microsoft.Maui.Controls.resources => 37
	i64 u0x224538d85ed15a82, ; 96: System.IO.Pipes => 156
	i64 u0x234b2420fe4b9bdc, ; 97: lib_K4os.Compression.LZ4.dll.so => 49
	i64 u0x237be844f1f812c7, ; 98: System.Threading.Thread.dll => 208
	i64 u0x23807c59646ec4f3, ; 99: lib_Microsoft.EntityFrameworkCore.dll.so => 55
	i64 u0x23852b3bdc9f7096, ; 100: System.Resources.ResourceManager => 179
	i64 u0x23cce13de11e9adc, ; 101: Lotus Spor.dll => 125
	i64 u0x2407aef2bbe8fadf, ; 102: System.Console => 139
	i64 u0x240abe014b27e7d3, ; 103: Xamarin.AndroidX.Core.dll => 101
	i64 u0x247619fe4413f8bf, ; 104: System.Runtime.Serialization.Primitives.dll => 188
	i64 u0x252073cc3caa62c2, ; 105: fr/Microsoft.Maui.Controls.resources.dll => 18
	i64 u0x2662c629b96b0b30, ; 106: lib_Xamarin.Kotlin.StdLib.dll.so => 121
	i64 u0x268c1439f13bcc29, ; 107: lib_Microsoft.Extensions.Primitives.dll.so => 67
	i64 u0x270a44600c921861, ; 108: System.IdentityModel.Tokens.Jwt => 88
	i64 u0x273f3515de5faf0d, ; 109: id/Microsoft.Maui.Controls.resources.dll => 23
	i64 u0x2742545f9094896d, ; 110: hr/Microsoft.Maui.Controls.resources => 21
	i64 u0x27b410442fad6cf1, ; 111: Java.Interop.dll => 220
	i64 u0x27b97e0d52c3034a, ; 112: System.Diagnostics.Debug => 141
	i64 u0x2801845a2c71fbfb, ; 113: System.Net.Primitives.dll => 164
	i64 u0x28e27b6a43116264, ; 114: lib_Lotus Spor.dll.so => 125
	i64 u0x2a128783efe70ba0, ; 115: uk/Microsoft.Maui.Controls.resources.dll => 39
	i64 u0x2a3b095612184159, ; 116: lib_System.Net.NetworkInformation.dll.so => 163
	i64 u0x2a6507a5ffabdf28, ; 117: System.Diagnostics.TraceSource.dll => 146
	i64 u0x2ad156c8e1354139, ; 118: fi/Microsoft.Maui.Controls.resources => 17
	i64 u0x2af298f63581d886, ; 119: System.Text.RegularExpressions.dll => 204
	i64 u0x2af615542f04da50, ; 120: System.IdentityModel.Tokens.Jwt.dll => 88
	i64 u0x2afc1c4f898552ee, ; 121: lib_System.Formats.Asn1.dll.so => 150
	i64 u0x2b148910ed40fbf9, ; 122: zh-Hant/Microsoft.Maui.Controls.resources.dll => 43
	i64 u0x2c8bd14bb93a7d82, ; 123: lib-pl-Microsoft.Maui.Controls.resources.dll.so => 30
	i64 u0x2cbd9262ca785540, ; 124: lib_System.Text.Encoding.CodePages.dll.so => 201
	i64 u0x2cc9e1fed6257257, ; 125: lib_System.Reflection.Emit.Lightweight.dll.so => 177
	i64 u0x2cd723e9fe623c7c, ; 126: lib_System.Private.Xml.Linq.dll.so => 174
	i64 u0x2ce66f4c8733e883, ; 127: pt-BR/Microsoft.Data.SqlClient.resources.dll => 6
	i64 u0x2d169d318a968379, ; 128: System.Threading.dll => 210
	i64 u0x2d47774b7d993f59, ; 129: sv/Microsoft.Maui.Controls.resources.dll => 36
	i64 u0x2db915caf23548d2, ; 130: System.Text.Json.dll => 93
	i64 u0x2e6f1f226821322a, ; 131: el/Microsoft.Maui.Controls.resources.dll => 15
	i64 u0x2f02f94df3200fe5, ; 132: System.Diagnostics.Process => 143
	i64 u0x2f2e98e1c89b1aff, ; 133: System.Xml.ReaderWriter => 214
	i64 u0x2f40b2521deba305, ; 134: lib_Microsoft.SqlServer.Server.dll.so => 81
	i64 u0x2f5911d9ba814e4e, ; 135: System.Diagnostics.Tracing => 147
	i64 u0x2feb4d2fcda05cfd, ; 136: Microsoft.Extensions.Caching.Abstractions.dll => 58
	i64 u0x309ee9eeec09a71e, ; 137: lib_Xamarin.AndroidX.Fragment.dll.so => 105
	i64 u0x309f2bedefa9a318, ; 138: Microsoft.IdentityModel.Abstractions => 70
	i64 u0x31195fef5d8fb552, ; 139: _Microsoft.Android.Resource.Designer.dll => 44
	i64 u0x32243413e774362a, ; 140: Xamarin.AndroidX.CardView.dll => 98
	i64 u0x3235427f8d12dae1, ; 141: lib_System.Drawing.Primitives.dll.so => 148
	i64 u0x329753a17a517811, ; 142: fr/Microsoft.Maui.Controls.resources => 18
	i64 u0x32aa989ff07a84ff, ; 143: lib_System.Xml.ReaderWriter.dll.so => 214
	i64 u0x33829542f112d59b, ; 144: System.Collections.Immutable => 130
	i64 u0x33a31443733849fe, ; 145: lib-es-Microsoft.Maui.Controls.resources.dll.so => 16
	i64 u0x341abc357fbb4ebf, ; 146: lib_System.Net.Sockets.dll.so => 167
	i64 u0x348d598f4054415e, ; 147: Microsoft.SqlServer.Server => 81
	i64 u0x34dfd74fe2afcf37, ; 148: Microsoft.Maui => 78
	i64 u0x34e292762d9615df, ; 149: cs/Microsoft.Maui.Controls.resources.dll => 12
	i64 u0x3508234247f48404, ; 150: Microsoft.Maui.Controls => 76
	i64 u0x353590da528c9d22, ; 151: System.ComponentModel.Annotations => 134
	i64 u0x3549870798b4cd30, ; 152: lib_Xamarin.AndroidX.ViewPager2.dll.so => 119
	i64 u0x355282fc1c909694, ; 153: Microsoft.Extensions.Configuration => 60
	i64 u0x355c649948d55d97, ; 154: lib_System.Runtime.Intrinsics.dll.so => 183
	i64 u0x36b2b50fdf589ae2, ; 155: System.Reflection.Emit.Lightweight => 177
	i64 u0x374ef46b06791af6, ; 156: System.Reflection.Primitives.dll => 178
	i64 u0x380134e03b1e160a, ; 157: System.Collections.Immutable.dll => 130
	i64 u0x38049b5c59b39324, ; 158: System.Runtime.CompilerServices.Unsafe => 180
	i64 u0x385c17636bb6fe6e, ; 159: Xamarin.AndroidX.CustomView.dll => 103
	i64 u0x38869c811d74050e, ; 160: System.Net.NameResolution.dll => 162
	i64 u0x38e93ec1c057cdf6, ; 161: Microsoft.IdentityModel.Protocols => 73
	i64 u0x39251dccb84bdcaa, ; 162: lib_System.Configuration.ConfigurationManager.dll.so => 85
	i64 u0x393c226616977fdb, ; 163: lib_Xamarin.AndroidX.ViewPager.dll.so => 118
	i64 u0x395e37c3334cf82a, ; 164: lib-ca-Microsoft.Maui.Controls.resources.dll.so => 11
	i64 u0x3ab5859054645f72, ; 165: System.Security.Cryptography.Primitives.dll => 197
	i64 u0x3b2c47fe17204e4d, ; 166: MySql.Data => 82
	i64 u0x3b860f9932505633, ; 167: lib_System.Text.Encoding.Extensions.dll.so => 202
	i64 u0x3bea9ebe8c027c01, ; 168: lib_Microsoft.IdentityModel.Tokens.dll.so => 75
	i64 u0x3c3aafb6b3a00bf6, ; 169: lib_System.Security.Cryptography.X509Certificates.dll.so => 198
	i64 u0x3c5f19e4acdcebd8, ; 170: lib_Microsoft.Data.SqlClient.dll.so => 54
	i64 u0x3c7c495f58ac5ee9, ; 171: Xamarin.Kotlin.StdLib => 121
	i64 u0x3cd9d281d402eb9b, ; 172: Xamarin.AndroidX.Browser.dll => 97
	i64 u0x3d196e782ed8c01a, ; 173: System.Data.SqlClient => 86
	i64 u0x3d2b1913edfc08d7, ; 174: lib_System.Threading.ThreadPool.dll.so => 209
	i64 u0x3d46f0b995082740, ; 175: System.Xml.Linq => 213
	i64 u0x3d9c2a242b040a50, ; 176: lib_Xamarin.AndroidX.Core.dll.so => 101
	i64 u0x3daa14724d8f58e8, ; 177: Google.Protobuf.dll => 48
	i64 u0x3e0b360b2840f096, ; 178: it/Microsoft.Data.SqlClient.resources => 3
	i64 u0x3f3c8f45ab6f28c7, ; 179: Microsoft.Identity.Client.Extensions.Msal.dll => 69
	i64 u0x3f510adf788828dd, ; 180: System.Threading.Tasks.Extensions => 206
	i64 u0x4019503dd3d938a1, ; 181: MySql.Data.dll => 82
	i64 u0x407a10bb4bf95829, ; 182: lib_Xamarin.AndroidX.Navigation.Common.dll.so => 111
	i64 u0x407ac43dee26bd5a, ; 183: lib_Azure.Identity.dll.so => 46
	i64 u0x407e0b13370378bc, ; 184: lib_MySql.Data.EntityFrameworkCore.dll.so => 83
	i64 u0x415e36f6b13ff6f3, ; 185: System.Configuration.ConfigurationManager.dll => 85
	i64 u0x41cab042be111c34, ; 186: lib_Xamarin.AndroidX.AppCompat.AppCompatResources.dll.so => 96
	i64 u0x423a9ecc4d905a88, ; 187: lib_System.Resources.ResourceManager.dll.so => 179
	i64 u0x43375950ec7c1b6a, ; 188: netstandard.dll => 218
	i64 u0x434c4e1d9284cdae, ; 189: Mono.Android.dll => 222
	i64 u0x43950f84de7cc79a, ; 190: pl/Microsoft.Maui.Controls.resources.dll => 30
	i64 u0x448bd33429269b19, ; 191: Microsoft.CSharp => 126
	i64 u0x4499fa3c8e494654, ; 192: lib_System.Runtime.Serialization.Primitives.dll.so => 188
	i64 u0x4515080865a951a5, ; 193: Xamarin.Kotlin.StdLib.dll => 121
	i64 u0x453c1277f85cf368, ; 194: lib_Microsoft.EntityFrameworkCore.Abstractions.dll.so => 56
	i64 u0x458d2df79ac57c1d, ; 195: lib_System.IdentityModel.Tokens.Jwt.dll.so => 88
	i64 u0x45c40276a42e283e, ; 196: System.Diagnostics.TraceSource => 146
	i64 u0x460e169e8680ae82, ; 197: MySql.Data.EntityFrameworkCore.dll => 83
	i64 u0x46a4213bc97fe5ae, ; 198: lib-ru-Microsoft.Maui.Controls.resources.dll.so => 34
	i64 u0x47358bd471172e1d, ; 199: lib_System.Xml.Linq.dll.so => 213
	i64 u0x4787a936949fcac2, ; 200: System.Memory.Data.dll => 89
	i64 u0x47daf4e1afbada10, ; 201: pt/Microsoft.Maui.Controls.resources => 32
	i64 u0x4953c088b9debf0a, ; 202: lib_System.Security.Permissions.dll.so => 92
	i64 u0x49e952f19a4e2022, ; 203: System.ObjectModel => 171
	i64 u0x4a5667b2462a664b, ; 204: lib_Xamarin.AndroidX.Navigation.UI.dll.so => 114
	i64 u0x4b576d47ac054f3c, ; 205: System.IO.FileSystem.AccessControl => 153
	i64 u0x4b7b6532ded934b7, ; 206: System.Text.Json => 93
	i64 u0x4b8f8ea3c2df6bb0, ; 207: System.ClientModel => 84
	i64 u0x4ca014ceac582c86, ; 208: Microsoft.EntityFrameworkCore.Relational.dll => 57
	i64 u0x4cc5f15266470798, ; 209: lib_Xamarin.AndroidX.Loader.dll.so => 110
	i64 u0x4cf6f67dc77aacd2, ; 210: System.Net.NetworkInformation.dll => 163
	i64 u0x4d479f968a05e504, ; 211: System.Linq.Expressions.dll => 157
	i64 u0x4d55a010ffc4faff, ; 212: System.Private.Xml => 175
	i64 u0x4d6001db23f8cd87, ; 213: lib_System.ClientModel.dll.so => 84
	i64 u0x4d95fccc1f67c7ca, ; 214: System.Runtime.Loader.dll => 184
	i64 u0x4dcf44c3c9b076a2, ; 215: it/Microsoft.Maui.Controls.resources.dll => 24
	i64 u0x4dd9247f1d2c3235, ; 216: Xamarin.AndroidX.Loader.dll => 110
	i64 u0x4e32f00cb0937401, ; 217: Mono.Android.Runtime => 221
	i64 u0x4e5eea4668ac2b18, ; 218: System.Text.Encoding.CodePages => 201
	i64 u0x4ebd0c4b82c5eefc, ; 219: lib_System.Threading.Channels.dll.so => 205
	i64 u0x4f21ee6ef9eb527e, ; 220: ca/Microsoft.Maui.Controls.resources => 11
	i64 u0x4ffd65baff757598, ; 221: Microsoft.IdentityModel.Tokens => 75
	i64 u0x50320f2a19424f3f, ; 222: lib-it-Microsoft.Data.SqlClient.resources.dll.so => 3
	i64 u0x5037f0be3c28c7a3, ; 223: lib_Microsoft.Maui.Controls.dll.so => 76
	i64 u0x5131bbe80989093f, ; 224: Xamarin.AndroidX.Lifecycle.ViewModel.Android.dll => 108
	i64 u0x5146d4e23aed3198, ; 225: ja/Microsoft.Data.SqlClient.resources => 4
	i64 u0x51bb8a2afe774e32, ; 226: System.Drawing => 149
	i64 u0x526ce79eb8e90527, ; 227: lib_System.Net.Primitives.dll.so => 164
	i64 u0x52829f00b4467c38, ; 228: lib_System.Data.Common.dll.so => 140
	i64 u0x5290402954d7bce0, ; 229: zh-Hans/Microsoft.Data.SqlClient.resources => 8
	i64 u0x529ffe06f39ab8db, ; 230: Xamarin.AndroidX.Core => 101
	i64 u0x52ff996554dbf352, ; 231: Microsoft.Maui.Graphics => 80
	i64 u0x535f7e40e8fef8af, ; 232: lib-sk-Microsoft.Maui.Controls.resources.dll.so => 35
	i64 u0x53978aac584c666e, ; 233: lib_System.Security.Cryptography.Cng.dll.so => 194
	i64 u0x53a96d5c86c9e194, ; 234: System.Net.NetworkInformation => 163
	i64 u0x53be1038a61e8d44, ; 235: System.Runtime.InteropServices.RuntimeInformation.dll => 181
	i64 u0x53c3014b9437e684, ; 236: lib-zh-HK-Microsoft.Maui.Controls.resources.dll.so => 41
	i64 u0x5435e6f049e9bc37, ; 237: System.Security.Claims.dll => 192
	i64 u0x54795225dd1587af, ; 238: lib_System.Runtime.dll.so => 190
	i64 u0x556e8b63b660ab8b, ; 239: Xamarin.AndroidX.Lifecycle.Common.Jvm.dll => 106
	i64 u0x5588627c9a108ec9, ; 240: System.Collections.Specialized => 132
	i64 u0x56442b99bc64bb47, ; 241: System.Runtime.Serialization.Xml.dll => 189
	i64 u0x571c5cfbec5ae8e2, ; 242: System.Private.Uri => 173
	i64 u0x579a06fed6eec900, ; 243: System.Private.CoreLib.dll => 219
	i64 u0x57c542c14049b66d, ; 244: System.Diagnostics.DiagnosticSource => 142
	i64 u0x58601b2dda4a27b9, ; 245: lib-ja-Microsoft.Maui.Controls.resources.dll.so => 25
	i64 u0x58688d9af496b168, ; 246: Microsoft.Extensions.DependencyInjection.dll => 62
	i64 u0x595a356d23e8da9a, ; 247: lib_Microsoft.CSharp.dll.so => 126
	i64 u0x5a70033ca9d003cb, ; 248: lib_System.Memory.Data.dll.so => 89
	i64 u0x5a89a886ae30258d, ; 249: lib_Xamarin.AndroidX.CoordinatorLayout.dll.so => 100
	i64 u0x5a8f6699f4a1caa9, ; 250: lib_System.Threading.dll.so => 210
	i64 u0x5ae9cd33b15841bf, ; 251: System.ComponentModel => 138
	i64 u0x5b54391bdc6fcfe6, ; 252: System.Private.DataContractSerialization => 172
	i64 u0x5b5f0e240a06a2a2, ; 253: da/Microsoft.Maui.Controls.resources.dll => 13
	i64 u0x5b608c01082a90a8, ; 254: K4os.Hash.xxHash => 51
	i64 u0x5bf46332cc09e9b2, ; 255: lib_System.Data.SqlClient.dll.so => 86
	i64 u0x5c393624b8176517, ; 256: lib_Microsoft.Extensions.Logging.dll.so => 64
	i64 u0x5d0a4a29b02d9d3c, ; 257: System.Net.WebHeaderCollection.dll => 169
	i64 u0x5d33da2f84c1de97, ; 258: lib-pt-BR-Microsoft.Data.SqlClient.resources.dll.so => 6
	i64 u0x5db0cbbd1028510e, ; 259: lib_System.Runtime.InteropServices.dll.so => 182
	i64 u0x5db30905d3e5013b, ; 260: Xamarin.AndroidX.Collection.Jvm.dll => 99
	i64 u0x5e467bc8f09ad026, ; 261: System.Collections.Specialized.dll => 132
	i64 u0x5ea92fdb19ec8c4c, ; 262: System.Text.Encodings.Web.dll => 203
	i64 u0x5eb8046dd40e9ac3, ; 263: System.ComponentModel.Primitives => 136
	i64 u0x5ec272d219c9aba4, ; 264: System.Security.Cryptography.Csp.dll => 195
	i64 u0x5f36ccf5c6a57e24, ; 265: System.Xml.ReaderWriter.dll => 214
	i64 u0x5f4294b9b63cb842, ; 266: System.Data.Common => 140
	i64 u0x5f9a2d823f664957, ; 267: lib-el-Microsoft.Maui.Controls.resources.dll.so => 15
	i64 u0x5fac98e0b37a5b9d, ; 268: System.Runtime.CompilerServices.Unsafe.dll => 180
	i64 u0x609f4b7b63d802d4, ; 269: lib_Microsoft.Extensions.DependencyInjection.dll.so => 62
	i64 u0x60cd4e33d7e60134, ; 270: Xamarin.KotlinX.Coroutines.Core.Jvm => 122
	i64 u0x60f62d786afcf130, ; 271: System.Memory => 160
	i64 u0x618073e67851e2a7, ; 272: lib_K4os.Compression.LZ4.Streams.dll.so => 50
	i64 u0x61be8d1299194243, ; 273: Microsoft.Maui.Controls.Xaml => 77
	i64 u0x61d2cba29557038f, ; 274: de/Microsoft.Maui.Controls.resources => 14
	i64 u0x61d88f399afb2f45, ; 275: lib_System.Runtime.Loader.dll.so => 184
	i64 u0x622eef6f9e59068d, ; 276: System.Private.CoreLib => 219
	i64 u0x63f1f6883c1e23c2, ; 277: lib_System.Collections.Immutable.dll.so => 130
	i64 u0x6400f68068c1e9f1, ; 278: Xamarin.Google.Android.Material.dll => 120
	i64 u0x640e3b14dbd325c2, ; 279: System.Security.Cryptography.Algorithms.dll => 193
	i64 u0x64877f6b1c4cf837, ; 280: Microsoft.Bcl.HashCode => 53
	i64 u0x65a51fb1cf95ad53, ; 281: ZstdSharp.dll => 124
	i64 u0x65ecac39144dd3cc, ; 282: Microsoft.Maui.Controls.dll => 76
	i64 u0x65ece51227bfa724, ; 283: lib_System.Runtime.Numerics.dll.so => 185
	i64 u0x6692e924eade1b29, ; 284: lib_System.Console.dll.so => 139
	i64 u0x66a4e5c6a3fb0bae, ; 285: lib_Xamarin.AndroidX.Lifecycle.ViewModel.Android.dll.so => 108
	i64 u0x66d13304ce1a3efa, ; 286: Xamarin.AndroidX.CursorAdapter => 102
	i64 u0x68558ec653afa616, ; 287: lib-da-Microsoft.Maui.Controls.resources.dll.so => 13
	i64 u0x6872ec7a2e36b1ac, ; 288: System.Drawing.Primitives.dll => 148
	i64 u0x68fbbbe2eb455198, ; 289: System.Formats.Asn1 => 150
	i64 u0x69063fc0ba8e6bdd, ; 290: he/Microsoft.Maui.Controls.resources.dll => 19
	i64 u0x6a4d7577b2317255, ; 291: System.Runtime.InteropServices.dll => 182
	i64 u0x6ace3b74b15ee4a4, ; 292: nb/Microsoft.Maui.Controls.resources => 28
	i64 u0x6d0a12b2adba20d8, ; 293: System.Security.Cryptography.ProtectedData.dll => 91
	i64 u0x6d12bfaa99c72b1f, ; 294: lib_Microsoft.Maui.Graphics.dll.so => 80
	i64 u0x6d70755158ca866e, ; 295: lib_System.ComponentModel.EventBasedAsync.dll.so => 135
	i64 u0x6d79993361e10ef2, ; 296: Microsoft.Extensions.Primitives => 67
	i64 u0x6d86d56b84c8eb71, ; 297: lib_Xamarin.AndroidX.CursorAdapter.dll.so => 102
	i64 u0x6d9bea6b3e895cf7, ; 298: Microsoft.Extensions.Primitives.dll => 67
	i64 u0x6e25a02c3833319a, ; 299: lib_Xamarin.AndroidX.Navigation.Fragment.dll.so => 112
	i64 u0x6fd2265da78b93a4, ; 300: lib_Microsoft.Maui.dll.so => 78
	i64 u0x6fdfc7de82c33008, ; 301: cs/Microsoft.Maui.Controls.resources => 12
	i64 u0x6ffc4967cc47ba57, ; 302: System.IO.FileSystem.Watcher.dll => 154
	i64 u0x70e99f48c05cb921, ; 303: tr/Microsoft.Maui.Controls.resources.dll => 38
	i64 u0x70fd3deda22442d2, ; 304: lib-nb-Microsoft.Maui.Controls.resources.dll.so => 28
	i64 u0x71a495ea3761dde8, ; 305: lib-it-Microsoft.Maui.Controls.resources.dll.so => 24
	i64 u0x71ad672adbe48f35, ; 306: System.ComponentModel.Primitives.dll => 136
	i64 u0x725f5a9e82a45c81, ; 307: System.Security.Cryptography.Encoding => 196
	i64 u0x72b1fb4109e08d7b, ; 308: lib-hr-Microsoft.Maui.Controls.resources.dll.so => 21
	i64 u0x73e4ce94e2eb6ffc, ; 309: lib_System.Memory.dll.so => 160
	i64 u0x755a91767330b3d4, ; 310: lib_Microsoft.Extensions.Configuration.dll.so => 60
	i64 u0x76012e7334db86e5, ; 311: lib_Xamarin.AndroidX.SavedState.dll.so => 116
	i64 u0x76ca07b878f44da0, ; 312: System.Runtime.Numerics.dll => 185
	i64 u0x777b4ed432c1e61e, ; 313: K4os.Compression.LZ4.Streams => 50
	i64 u0x77f8a4acc2fdc449, ; 314: System.Security.Cryptography.Cng.dll => 194
	i64 u0x780bc73597a503a9, ; 315: lib-ms-Microsoft.Maui.Controls.resources.dll.so => 27
	i64 u0x783606d1e53e7a1a, ; 316: th/Microsoft.Maui.Controls.resources.dll => 37
	i64 u0x7841c47b741b9f64, ; 317: System.Security.Permissions => 92
	i64 u0x7891b42e012cde6f, ; 318: Lotus Spor => 125
	i64 u0x78a45e51311409b6, ; 319: Xamarin.AndroidX.Fragment.dll => 105
	i64 u0x79eb916f2d11e1f0, ; 320: zh-Hans/Microsoft.Data.SqlClient.resources.dll => 8
	i64 u0x7adb8da2ac89b647, ; 321: fi/Microsoft.Maui.Controls.resources.dll => 17
	i64 u0x7b4927e421291c41, ; 322: Microsoft.IdentityModel.JsonWebTokens.dll => 71
	i64 u0x7bef86a4335c4870, ; 323: System.ComponentModel.TypeConverter => 137
	i64 u0x7c0820144cd34d6a, ; 324: sk/Microsoft.Maui.Controls.resources.dll => 35
	i64 u0x7c2a0bd1e0f988fc, ; 325: lib-de-Microsoft.Maui.Controls.resources.dll.so => 14
	i64 u0x7c41d387501568ba, ; 326: System.Net.WebClient.dll => 168
	i64 u0x7d649b75d580bb42, ; 327: ms/Microsoft.Maui.Controls.resources.dll => 27
	i64 u0x7d8ee2bdc8e3aad1, ; 328: System.Numerics.Vectors => 170
	i64 u0x7dfc3d6d9d8d7b70, ; 329: System.Collections => 133
	i64 u0x7e1f8f575a3599cb, ; 330: BouncyCastle.Cryptography.dll => 47
	i64 u0x7e2e564fa2f76c65, ; 331: lib_System.Diagnostics.Tracing.dll.so => 147
	i64 u0x7e302e110e1e1346, ; 332: lib_System.Security.Claims.dll.so => 192
	i64 u0x7e946809d6008ef2, ; 333: lib_System.ObjectModel.dll.so => 171
	i64 u0x7ecc13347c8fd849, ; 334: lib_System.ComponentModel.dll.so => 138
	i64 u0x7f00ddd9b9ca5a13, ; 335: Xamarin.AndroidX.ViewPager.dll => 118
	i64 u0x7f9351cd44b1273f, ; 336: Microsoft.Extensions.Configuration.Abstractions => 61
	i64 u0x7fae0ef4dc4770fe, ; 337: Microsoft.Identity.Client => 68
	i64 u0x7fbd557c99b3ce6f, ; 338: lib_Xamarin.AndroidX.Lifecycle.LiveData.Core.dll.so => 107
	i64 u0x812c069d5cdecc17, ; 339: System.dll => 217
	i64 u0x81ab745f6c0f5ce6, ; 340: zh-Hant/Microsoft.Maui.Controls.resources => 43
	i64 u0x82075fdf49c26af2, ; 341: ZstdSharp => 124
	i64 u0x8277f2be6b5ce05f, ; 342: Xamarin.AndroidX.AppCompat => 95
	i64 u0x828f06563b30bc50, ; 343: lib_Xamarin.AndroidX.CardView.dll.so => 98
	i64 u0x82df8f5532a10c59, ; 344: lib_System.Drawing.dll.so => 149
	i64 u0x82f6403342e12049, ; 345: uk/Microsoft.Maui.Controls.resources => 39
	i64 u0x83a7afd2c49adc86, ; 346: lib_Microsoft.IdentityModel.Abstractions.dll.so => 70
	i64 u0x83c14ba66c8e2b8c, ; 347: zh-Hans/Microsoft.Maui.Controls.resources => 42
	i64 u0x84ae73148a4557d2, ; 348: lib_System.IO.Pipes.dll.so => 156
	i64 u0x84b01102c12a9232, ; 349: System.Runtime.Serialization.Json.dll => 187
	i64 u0x84cd5cdec0f54bcc, ; 350: lib_Microsoft.EntityFrameworkCore.Relational.dll.so => 57
	i64 u0x8528b82bdbc15371, ; 351: ko/Microsoft.Data.SqlClient.resources => 5
	i64 u0x86a909228dc7657b, ; 352: lib-zh-Hant-Microsoft.Maui.Controls.resources.dll.so => 43
	i64 u0x86b3e00c36b84509, ; 353: Microsoft.Extensions.Configuration.dll => 60
	i64 u0x86b62cb077ec4fd7, ; 354: System.Runtime.Serialization.Xml => 189
	i64 u0x87c4b8a492b176ad, ; 355: Microsoft.EntityFrameworkCore.Abstractions => 56
	i64 u0x87c69b87d9283884, ; 356: lib_System.Threading.Thread.dll.so => 208
	i64 u0x87f6569b25707834, ; 357: System.IO.Compression.Brotli.dll => 151
	i64 u0x8842b3a5d2d3fb36, ; 358: Microsoft.Maui.Essentials => 79
	i64 u0x88bda98e0cffb7a9, ; 359: lib_Xamarin.KotlinX.Coroutines.Core.Jvm.dll.so => 122
	i64 u0x8930322c7bd8f768, ; 360: netstandard => 218
	i64 u0x897a606c9e39c75f, ; 361: lib_System.ComponentModel.Primitives.dll.so => 136
	i64 u0x89c5188089ec2cd5, ; 362: lib_System.Runtime.InteropServices.RuntimeInformation.dll.so => 181
	i64 u0x8a399a706fcbce4b, ; 363: Microsoft.Extensions.Caching.Abstractions => 58
	i64 u0x8ad229ea26432ee2, ; 364: Xamarin.AndroidX.Loader => 110
	i64 u0x8b4ff5d0fdd5faa1, ; 365: lib_System.Diagnostics.DiagnosticSource.dll.so => 142
	i64 u0x8b541d476eb3774c, ; 366: System.Security.Principal.Windows => 200
	i64 u0x8b8d01333a96d0b5, ; 367: System.Diagnostics.Process.dll => 143
	i64 u0x8b9ceca7acae3451, ; 368: lib-he-Microsoft.Maui.Controls.resources.dll.so => 19
	i64 u0x8c1bafb2ed25af5b, ; 369: K4os.Compression.LZ4.Streams.dll => 50
	i64 u0x8c53ae18581b14f0, ; 370: Azure.Core => 45
	i64 u0x8cdfdb4ce85fb925, ; 371: lib_System.Security.Principal.Windows.dll.so => 200
	i64 u0x8d0f420977c2c1c7, ; 372: Xamarin.AndroidX.CursorAdapter.dll => 102
	i64 u0x8d7b8ab4b3310ead, ; 373: System.Threading => 210
	i64 u0x8da188285aadfe8e, ; 374: System.Collections.Concurrent => 129
	i64 u0x8e937db395a74375, ; 375: lib_Microsoft.Identity.Client.dll.so => 68
	i64 u0x8ed3cdd722b4d782, ; 376: System.Diagnostics.EventLog => 87
	i64 u0x8ed807bfe9858dfc, ; 377: Xamarin.AndroidX.Navigation.Common => 111
	i64 u0x8ee08b8194a30f48, ; 378: lib-hi-Microsoft.Maui.Controls.resources.dll.so => 20
	i64 u0x8ef7601039857a44, ; 379: lib-ro-Microsoft.Maui.Controls.resources.dll.so => 33
	i64 u0x8f32c6f611f6ffab, ; 380: pt/Microsoft.Maui.Controls.resources.dll => 32
	i64 u0x8f8829d21c8985a4, ; 381: lib-pt-BR-Microsoft.Maui.Controls.resources.dll.so => 31
	i64 u0x90263f8448b8f572, ; 382: lib_System.Diagnostics.TraceSource.dll.so => 146
	i64 u0x903101b46fb73a04, ; 383: _Microsoft.Android.Resource.Designer => 44
	i64 u0x90393bd4865292f3, ; 384: lib_System.IO.Compression.dll.so => 152
	i64 u0x905e2b8e7ae91ae6, ; 385: System.Threading.Tasks.Extensions.dll => 206
	i64 u0x90634f86c5ebe2b5, ; 386: Xamarin.AndroidX.Lifecycle.ViewModel.Android => 108
	i64 u0x907b636704ad79ef, ; 387: lib_Microsoft.Maui.Controls.Xaml.dll.so => 77
	i64 u0x91418dc638b29e68, ; 388: lib_Xamarin.AndroidX.CustomView.dll.so => 103
	i64 u0x9157bd523cd7ed36, ; 389: lib_System.Text.Json.dll.so => 93
	i64 u0x91a74f07b30d37e2, ; 390: System.Linq.dll => 159
	i64 u0x91fa41a87223399f, ; 391: ca/Microsoft.Maui.Controls.resources.dll => 11
	i64 u0x93489853b6098685, ; 392: es/Microsoft.Data.SqlClient.resources.dll => 1
	i64 u0x93cfa73ab28d6e35, ; 393: ms/Microsoft.Maui.Controls.resources => 27
	i64 u0x944077d8ca3c6580, ; 394: System.IO.Compression.dll => 152
	i64 u0x948d746a7702861f, ; 395: Microsoft.IdentityModel.Logging.dll => 72
	i64 u0x9502fd818eed2359, ; 396: lib_Microsoft.IdentityModel.Protocols.OpenIdConnect.dll.so => 74
	i64 u0x9564283c37ed59a9, ; 397: lib_Microsoft.IdentityModel.Logging.dll.so => 72
	i64 u0x967fc325e09bfa8c, ; 398: es/Microsoft.Maui.Controls.resources => 16
	i64 u0x96e49b31fe33d427, ; 399: Microsoft.Identity.Client.Extensions.Msal => 69
	i64 u0x9732d8dbddea3d9a, ; 400: id/Microsoft.Maui.Controls.resources => 23
	i64 u0x978be80e5210d31b, ; 401: Microsoft.Maui.Graphics.dll => 80
	i64 u0x97b8c771ea3e4220, ; 402: System.ComponentModel.dll => 138
	i64 u0x97e144c9d3c6976e, ; 403: System.Collections.Concurrent.dll => 129
	i64 u0x991d510397f92d9d, ; 404: System.Linq.Expressions => 157
	i64 u0x99868af5d93ecaeb, ; 405: lib_K4os.Hash.xxHash.dll.so => 51
	i64 u0x99a00ca5270c6878, ; 406: Xamarin.AndroidX.Navigation.Runtime => 113
	i64 u0x99cdc6d1f2d3a72f, ; 407: ko/Microsoft.Maui.Controls.resources.dll => 26
	i64 u0x9a0cc42c6f36dfc9, ; 408: lib_Microsoft.IdentityModel.Protocols.dll.so => 73
	i64 u0x9b211a749105beac, ; 409: System.Transactions.Local => 211
	i64 u0x9c244ac7cda32d26, ; 410: System.Security.Cryptography.X509Certificates.dll => 198
	i64 u0x9d5dbcf5a48583fe, ; 411: lib_Xamarin.AndroidX.Activity.dll.so => 94
	i64 u0x9d74dee1a7725f34, ; 412: Microsoft.Extensions.Configuration.Abstractions.dll => 61
	i64 u0x9e4534b6adaf6e84, ; 413: nl/Microsoft.Maui.Controls.resources => 29
	i64 u0x9e4b95dec42769f7, ; 414: System.Diagnostics.Debug.dll => 141
	i64 u0x9eaf1efdf6f7267e, ; 415: Xamarin.AndroidX.Navigation.Common.dll => 111
	i64 u0x9ef542cf1f78c506, ; 416: Xamarin.AndroidX.Lifecycle.LiveData.Core => 107
	i64 u0x9ffbb6b1434ad2df, ; 417: Microsoft.Identity.Client.dll => 68
	i64 u0xa0d8259f4cc284ec, ; 418: lib_System.Security.Cryptography.dll.so => 199
	i64 u0xa1440773ee9d341e, ; 419: Xamarin.Google.Android.Material => 120
	i64 u0xa1b9d7c27f47219f, ; 420: Xamarin.AndroidX.Navigation.UI.dll => 114
	i64 u0xa2572680829d2c7c, ; 421: System.IO.Pipelines.dll => 155
	i64 u0xa46aa1eaa214539b, ; 422: ko/Microsoft.Maui.Controls.resources => 26
	i64 u0xa4edc8f2ceae241a, ; 423: System.Data.Common.dll => 140
	i64 u0xa5494f40f128ce6a, ; 424: System.Runtime.Serialization.Formatters.dll => 186
	i64 u0xa5b7152421ed6d98, ; 425: lib_System.IO.FileSystem.Watcher.dll.so => 154
	i64 u0xa5ce5c755bde8cb8, ; 426: lib_System.Security.Cryptography.Csp.dll.so => 195
	i64 u0xa5e599d1e0524750, ; 427: System.Numerics.Vectors.dll => 170
	i64 u0xa5f1ba49b85dd355, ; 428: System.Security.Cryptography.dll => 199
	i64 u0xa61975a5a37873ea, ; 429: lib_System.Xml.XmlSerializer.dll.so => 216
	i64 u0xa64476a892d76457, ; 430: lib_MySql.Data.dll.so => 82
	i64 u0xa67dbee13e1df9ca, ; 431: Xamarin.AndroidX.SavedState.dll => 116
	i64 u0xa68a420042bb9b1f, ; 432: Xamarin.AndroidX.DrawerLayout.dll => 104
	i64 u0xa71fe7d6f6f93efd, ; 433: Microsoft.Data.SqlClient => 54
	i64 u0xa763fbb98df8d9fb, ; 434: lib_Microsoft.Win32.Primitives.dll.so => 127
	i64 u0xa78ce3745383236a, ; 435: Xamarin.AndroidX.Lifecycle.Common.Jvm => 106
	i64 u0xa7c31b56b4dc7b33, ; 436: hu/Microsoft.Maui.Controls.resources => 22
	i64 u0xa8e6320dd07580ef, ; 437: lib_Microsoft.IdentityModel.JsonWebTokens.dll.so => 71
	i64 u0xaa2219c8e3449ff5, ; 438: Microsoft.Extensions.Logging.Abstractions => 65
	i64 u0xaa443ac34067eeef, ; 439: System.Private.Xml.dll => 175
	i64 u0xaa52de307ef5d1dd, ; 440: System.Net.Http => 161
	i64 u0xaa9a7b0214a5cc5c, ; 441: System.Diagnostics.StackTrace.dll => 144
	i64 u0xaaaf86367285a918, ; 442: Microsoft.Extensions.DependencyInjection.Abstractions.dll => 63
	i64 u0xaaf84bb3f052a265, ; 443: el/Microsoft.Maui.Controls.resources => 15
	i64 u0xab9c1b2687d86b0b, ; 444: lib_System.Linq.Expressions.dll.so => 157
	i64 u0xac2af3fa195a15ce, ; 445: System.Runtime.Numerics => 185
	i64 u0xac5376a2a538dc10, ; 446: Xamarin.AndroidX.Lifecycle.LiveData.Core.dll => 107
	i64 u0xac65e40f62b6b90e, ; 447: Google.Protobuf => 48
	i64 u0xac79c7e46047ad98, ; 448: System.Security.Principal.Windows.dll => 200
	i64 u0xac98d31068e24591, ; 449: System.Xml.XDocument => 215
	i64 u0xacd46e002c3ccb97, ; 450: ro/Microsoft.Maui.Controls.resources => 33
	i64 u0xacf42eea7ef9cd12, ; 451: System.Threading.Channels => 205
	i64 u0xad89c07347f1bad6, ; 452: nl/Microsoft.Maui.Controls.resources.dll => 29
	i64 u0xadbb53caf78a79d2, ; 453: System.Web.HttpUtility => 212
	i64 u0xadc90ab061a9e6e4, ; 454: System.ComponentModel.TypeConverter.dll => 137
	i64 u0xadf511667bef3595, ; 455: System.Net.Security => 166
	i64 u0xae0aaa94fdcfce0f, ; 456: System.ComponentModel.EventBasedAsync.dll => 135
	i64 u0xae282bcd03739de7, ; 457: Java.Interop => 220
	i64 u0xae53579c90db1107, ; 458: System.ObjectModel.dll => 171
	i64 u0xafe29f45095518e7, ; 459: lib_Xamarin.AndroidX.Lifecycle.ViewModelSavedState.dll.so => 109
	i64 u0xb05cc42cd94c6d9d, ; 460: lib-sv-Microsoft.Maui.Controls.resources.dll.so => 36
	i64 u0xb0bb43dc52ea59f9, ; 461: System.Diagnostics.Tracing.dll => 147
	i64 u0xb1dd05401aa8ee63, ; 462: System.Security.AccessControl => 191
	i64 u0xb220631954820169, ; 463: System.Text.RegularExpressions => 204
	i64 u0xb2376e1dbf8b4ed7, ; 464: System.Security.Cryptography.Csp => 195
	i64 u0xb2a3f67f3bf29fce, ; 465: da/Microsoft.Maui.Controls.resources => 13
	i64 u0xb398860d6ed7ba2f, ; 466: System.Security.Cryptography.ProtectedData => 91
	i64 u0xb3f0a0fcda8d3ebc, ; 467: Xamarin.AndroidX.CardView => 98
	i64 u0xb46be1aa6d4fff93, ; 468: hi/Microsoft.Maui.Controls.resources => 20
	i64 u0xb477491be13109d8, ; 469: ar/Microsoft.Maui.Controls.resources => 10
	i64 u0xb4bd7015ecee9d86, ; 470: System.IO.Pipelines => 155
	i64 u0xb4c53d9749c5f226, ; 471: lib_System.IO.FileSystem.AccessControl.dll.so => 153
	i64 u0xb5c38bf497a4cfe2, ; 472: lib_System.Threading.Tasks.dll.so => 207
	i64 u0xb5c7fcdafbc67ee4, ; 473: Microsoft.Extensions.Logging.Abstractions.dll => 65
	i64 u0xb5ea31d5244c6626, ; 474: System.Threading.ThreadPool.dll => 209
	i64 u0xb7212c4683a94afe, ; 475: System.Drawing.Primitives => 148
	i64 u0xb7b7753d1f319409, ; 476: sv/Microsoft.Maui.Controls.resources => 36
	i64 u0xb81a2c6e0aee50fe, ; 477: lib_System.Private.CoreLib.dll.so => 219
	i64 u0xb9185c33a1643eed, ; 478: Microsoft.CSharp.dll => 126
	i64 u0xb9f64d3b230def68, ; 479: lib-pt-Microsoft.Maui.Controls.resources.dll.so => 32
	i64 u0xb9fc3c8a556e3691, ; 480: ja/Microsoft.Maui.Controls.resources => 25
	i64 u0xba4670aa94a2b3c6, ; 481: lib_System.Xml.XDocument.dll.so => 215
	i64 u0xba48785529705af9, ; 482: System.Collections.dll => 133
	i64 u0xbadbc0a44214b54e, ; 483: K4os.Compression.LZ4 => 49
	i64 u0xbb65706fde942ce3, ; 484: System.Net.Sockets => 167
	i64 u0xbb8c8d165ef11460, ; 485: lib_Microsoft.Identity.Client.Extensions.Msal.dll.so => 69
	i64 u0xbbd180354b67271a, ; 486: System.Runtime.Serialization.Formatters => 186
	i64 u0xbcd22b365b764643, ; 487: lib-zh-Hans-Microsoft.Data.SqlClient.resources.dll.so => 8
	i64 u0xbcfa7c134d2089f3, ; 488: System.Runtime.Caching => 90
	i64 u0xbd0aaf9dbfcc3376, ; 489: fr/Microsoft.Data.SqlClient.resources.dll => 2
	i64 u0xbd0e2c0d55246576, ; 490: System.Net.Http.dll => 161
	i64 u0xbd3c2d7a8325e11b, ; 491: lib-fr-Microsoft.Data.SqlClient.resources.dll.so => 2
	i64 u0xbd437a2cdb333d0d, ; 492: Xamarin.AndroidX.ViewPager2 => 119
	i64 u0xbd4aef17dbfb0390, ; 493: ru/Microsoft.Data.SqlClient.resources => 7
	i64 u0xbd5d0b88d3d647a5, ; 494: lib_Xamarin.AndroidX.Browser.dll.so => 97
	i64 u0xbd877b14d0b56392, ; 495: System.Runtime.Intrinsics.dll => 183
	i64 u0xbe65a49036345cf4, ; 496: lib_System.Buffers.dll.so => 128
	i64 u0xbee38d4a88835966, ; 497: Xamarin.AndroidX.AppCompat.AppCompatResources => 96
	i64 u0xc040a4ab55817f58, ; 498: ar/Microsoft.Maui.Controls.resources.dll => 10
	i64 u0xc0d928351ab5ca77, ; 499: System.Console.dll => 139
	i64 u0xc0f5a221a9383aea, ; 500: System.Runtime.Intrinsics => 183
	i64 u0xc12b8b3afa48329c, ; 501: lib_System.Linq.dll.so => 159
	i64 u0xc1c2cb7af77b8858, ; 502: Microsoft.EntityFrameworkCore => 55
	i64 u0xc1ff9ae3cdb6e1e6, ; 503: Xamarin.AndroidX.Activity.dll => 94
	i64 u0xc2260e1da1054ac1, ; 504: lib_BouncyCastle.Cryptography.dll.so => 47
	i64 u0xc26c064effb1dea9, ; 505: System.Buffers.dll => 128
	i64 u0xc278de356ad8a9e3, ; 506: Microsoft.IdentityModel.Logging => 72
	i64 u0xc28c50f32f81cc73, ; 507: ja/Microsoft.Maui.Controls.resources.dll => 25
	i64 u0xc2a3bca55b573141, ; 508: System.IO.FileSystem.Watcher => 154
	i64 u0xc2bcfec99f69365e, ; 509: Xamarin.AndroidX.ViewPager2.dll => 119
	i64 u0xc30270de06c08cbb, ; 510: lib_Microsoft.Bcl.HashCode.dll.so => 53
	i64 u0xc30b52815b58ac2c, ; 511: lib_System.Runtime.Serialization.Xml.dll.so => 189
	i64 u0xc463e077917aa21d, ; 512: System.Runtime.Serialization.Json => 187
	i64 u0xc472ce300460ccb6, ; 513: Microsoft.EntityFrameworkCore.dll => 55
	i64 u0xc4d3858ed4d08512, ; 514: Xamarin.AndroidX.Lifecycle.ViewModelSavedState.dll => 109
	i64 u0xc4d69851fe06342f, ; 515: lib_Microsoft.Extensions.Caching.Memory.dll.so => 59
	i64 u0xc50fded0ded1418c, ; 516: lib_System.ComponentModel.TypeConverter.dll.so => 137
	i64 u0xc519125d6bc8fb11, ; 517: lib_System.Net.Requests.dll.so => 165
	i64 u0xc5293b19e4dc230e, ; 518: Xamarin.AndroidX.Navigation.Fragment => 112
	i64 u0xc5325b2fcb37446f, ; 519: lib_System.Private.Xml.dll.so => 175
	i64 u0xc583d8477b5d3bac, ; 520: zh-Hant/Microsoft.Data.SqlClient.resources.dll => 9
	i64 u0xc5a0f4b95a699af7, ; 521: lib_System.Private.Uri.dll.so => 173
	i64 u0xc5cdcd5b6277579e, ; 522: lib_System.Security.Cryptography.Algorithms.dll.so => 193
	i64 u0xc6a4665a88c57225, ; 523: lib_ZstdSharp.dll.so => 124
	i64 u0xc7c01e7d7c93a110, ; 524: System.Text.Encoding.Extensions.dll => 202
	i64 u0xc7ce851898a4548e, ; 525: lib_System.Web.HttpUtility.dll.so => 212
	i64 u0xc858a28d9ee5a6c5, ; 526: lib_System.Collections.Specialized.dll.so => 132
	i64 u0xc9c62c8f354ac568, ; 527: lib_System.Diagnostics.TextWriterTraceListener.dll.so => 145
	i64 u0xca32340d8d54dcd5, ; 528: Microsoft.Extensions.Caching.Memory.dll => 59
	i64 u0xca3a723e7342c5b6, ; 529: lib-tr-Microsoft.Maui.Controls.resources.dll.so => 38
	i64 u0xcab3493c70141c2d, ; 530: pl/Microsoft.Maui.Controls.resources => 30
	i64 u0xcacfddc9f7c6de76, ; 531: ro/Microsoft.Maui.Controls.resources.dll => 33
	i64 u0xcb45618372c47127, ; 532: Microsoft.EntityFrameworkCore.Relational => 57
	i64 u0xcbd4fdd9cef4a294, ; 533: lib__Microsoft.Android.Resource.Designer.dll.so => 44
	i64 u0xcc182c3afdc374d6, ; 534: Microsoft.Bcl.AsyncInterfaces => 52
	i64 u0xcc2876b32ef2794c, ; 535: lib_System.Text.RegularExpressions.dll.so => 204
	i64 u0xcc5c3bb714c4561e, ; 536: Xamarin.KotlinX.Coroutines.Core.Jvm.dll => 122
	i64 u0xcc76886e09b88260, ; 537: Xamarin.KotlinX.Serialization.Core.Jvm.dll => 123
	i64 u0xccf25c4b634ccd3a, ; 538: zh-Hans/Microsoft.Maui.Controls.resources.dll => 42
	i64 u0xcd10a42808629144, ; 539: System.Net.Requests => 165
	i64 u0xcd3586b93136841e, ; 540: lib_System.Runtime.Caching.dll.so => 90
	i64 u0xcdd0c48b6937b21c, ; 541: Xamarin.AndroidX.SwipeRefreshLayout => 117
	i64 u0xceb28d385f84f441, ; 542: Azure.Core.dll => 45
	i64 u0xcf140ed700bc8e66, ; 543: Microsoft.SqlServer.Server.dll => 81
	i64 u0xcf23d8093f3ceadf, ; 544: System.Diagnostics.DiagnosticSource.dll => 142
	i64 u0xcf8fc898f98b0d34, ; 545: System.Private.Xml.Linq => 174
	i64 u0xd063299fcfc0c93f, ; 546: lib_System.Runtime.Serialization.Json.dll.so => 187
	i64 u0xd0de8a113e976700, ; 547: System.Diagnostics.TextWriterTraceListener => 145
	i64 u0xd1194e1d8a8de83c, ; 548: lib_Xamarin.AndroidX.Lifecycle.Common.Jvm.dll.so => 106
	i64 u0xd22a0c4630f2fe66, ; 549: lib_System.Security.Cryptography.ProtectedData.dll.so => 91
	i64 u0xd2dffb59201927bd, ; 550: de/Microsoft.Data.SqlClient.resources.dll => 0
	i64 u0xd333d0af9e423810, ; 551: System.Runtime.InteropServices => 182
	i64 u0xd33a415cb4278969, ; 552: System.Security.Cryptography.Encoding.dll => 196
	i64 u0xd3426d966bb704f5, ; 553: Xamarin.AndroidX.AppCompat.AppCompatResources.dll => 96
	i64 u0xd3651b6fc3125825, ; 554: System.Private.Uri.dll => 173
	i64 u0xd373685349b1fe8b, ; 555: Microsoft.Extensions.Logging.dll => 64
	i64 u0xd3801faafafb7698, ; 556: System.Private.DataContractSerialization.dll => 172
	i64 u0xd38905f50c6895e1, ; 557: MySql.Data.EntityFrameworkCore => 83
	i64 u0xd3e4c8d6a2d5d470, ; 558: it/Microsoft.Maui.Controls.resources => 24
	i64 u0xd42655883bb8c19f, ; 559: Microsoft.EntityFrameworkCore.Abstractions.dll => 56
	i64 u0xd4645626dffec99d, ; 560: lib_Microsoft.Extensions.DependencyInjection.Abstractions.dll.so => 63
	i64 u0xd5507e11a2b2839f, ; 561: Xamarin.AndroidX.Lifecycle.ViewModelSavedState => 109
	i64 u0xd5858610826f1c08, ; 562: lib-ru-Microsoft.Data.SqlClient.resources.dll.so => 7
	i64 u0xd6694f8359737e4e, ; 563: Xamarin.AndroidX.SavedState => 116
	i64 u0xd6d21782156bc35b, ; 564: Xamarin.AndroidX.SwipeRefreshLayout.dll => 117
	i64 u0xd72329819cbbbc44, ; 565: lib_Microsoft.Extensions.Configuration.Abstractions.dll.so => 61
	i64 u0xd72c760af136e863, ; 566: System.Xml.XmlSerializer.dll => 216
	i64 u0xd7b3764ada9d341d, ; 567: lib_Microsoft.Extensions.Logging.Abstractions.dll.so => 65
	i64 u0xd9e245a1762ddad5, ; 568: BouncyCastle.Cryptography => 47
	i64 u0xda1dfa4c534a9251, ; 569: Microsoft.Extensions.DependencyInjection => 62
	i64 u0xdad05a11827959a3, ; 570: System.Collections.NonGeneric.dll => 131
	i64 u0xdb5383ab5865c007, ; 571: lib-vi-Microsoft.Maui.Controls.resources.dll.so => 40
	i64 u0xdb58816721c02a59, ; 572: lib_System.Reflection.Emit.ILGeneration.dll.so => 176
	i64 u0xdbeda89f832aa805, ; 573: vi/Microsoft.Maui.Controls.resources.dll => 40
	i64 u0xdbf2a779fbc3ac31, ; 574: System.Transactions.Local.dll => 211
	i64 u0xdbf9607a441b4505, ; 575: System.Linq => 159
	i64 u0xdc75032002d1a212, ; 576: lib_System.Transactions.Local.dll.so => 211
	i64 u0xdca8be7403f92d4f, ; 577: lib_System.Linq.Queryable.dll.so => 158
	i64 u0xdce2c53525640bf3, ; 578: Microsoft.Extensions.Logging => 64
	i64 u0xdd2b722d78ef5f43, ; 579: System.Runtime.dll => 190
	i64 u0xdd67031857c72f96, ; 580: lib_System.Text.Encodings.Web.dll.so => 203
	i64 u0xdde30e6b77aa6f6c, ; 581: lib-zh-Hans-Microsoft.Maui.Controls.resources.dll.so => 42
	i64 u0xde110ae80fa7c2e2, ; 582: System.Xml.XDocument.dll => 215
	i64 u0xde572c2b2fb32f93, ; 583: lib_System.Threading.Tasks.Extensions.dll.so => 206
	i64 u0xde8769ebda7d8647, ; 584: hr/Microsoft.Maui.Controls.resources.dll => 21
	i64 u0xdf35b6d818902893, ; 585: K4os.Hash.xxHash.dll => 51
	i64 u0xe0142572c095a480, ; 586: Xamarin.AndroidX.AppCompat.dll => 95
	i64 u0xe02f89350ec78051, ; 587: Xamarin.AndroidX.CoordinatorLayout.dll => 100
	i64 u0xe0ea30f1ac5b7731, ; 588: ko/Microsoft.Data.SqlClient.resources.dll => 5
	i64 u0xe0ee2e61123c1478, ; 589: lib-es-Microsoft.Data.SqlClient.resources.dll.so => 1
	i64 u0xe10b760bb1462e7a, ; 590: lib_System.Security.Cryptography.Primitives.dll.so => 197
	i64 u0xe12265280d0b036d, ; 591: fr/Microsoft.Data.SqlClient.resources => 2
	i64 u0xe192a588d4410686, ; 592: lib_System.IO.Pipelines.dll.so => 155
	i64 u0xe1a08bd3fa539e0d, ; 593: System.Runtime.Loader => 184
	i64 u0xe1b52f9f816c70ef, ; 594: System.Private.Xml.Linq.dll => 174
	i64 u0xe1ecfdb7fff86067, ; 595: System.Net.Security.dll => 166
	i64 u0xe22fa4c9c645db62, ; 596: System.Diagnostics.TextWriterTraceListener.dll => 145
	i64 u0xe2420585aeceb728, ; 597: System.Net.Requests.dll => 165
	i64 u0xe29b73bc11392966, ; 598: lib-id-Microsoft.Maui.Controls.resources.dll.so => 23
	i64 u0xe2e426c7714fa0bc, ; 599: Microsoft.Win32.Primitives.dll => 127
	i64 u0xe3811d68d4fe8463, ; 600: pt-BR/Microsoft.Maui.Controls.resources.dll => 31
	i64 u0xe3b7cbae5ad66c75, ; 601: lib_System.Security.Cryptography.Encoding.dll.so => 196
	i64 u0xe494f7ced4ecd10a, ; 602: hu/Microsoft.Maui.Controls.resources.dll => 22
	i64 u0xe4a9b1e40d1e8917, ; 603: lib-fi-Microsoft.Maui.Controls.resources.dll.so => 17
	i64 u0xe4f74a0b5bf9703f, ; 604: System.Runtime.Serialization.Primitives => 188
	i64 u0xe5434e8a119ceb69, ; 605: lib_Mono.Android.dll.so => 222
	i64 u0xe57d22ca4aeb4900, ; 606: System.Configuration.ConfigurationManager => 85
	i64 u0xe79d45aa815dab7f, ; 607: System.Runtime.Caching.dll => 90
	i64 u0xe7e03cc18dcdeb49, ; 608: lib_System.Diagnostics.StackTrace.dll.so => 144
	i64 u0xe89a2a9ef110899b, ; 609: System.Drawing.dll => 149
	i64 u0xed6ef763c6fb395f, ; 610: System.Diagnostics.EventLog.dll => 87
	i64 u0xedc4817167106c23, ; 611: System.Net.Sockets.dll => 167
	i64 u0xedc632067fb20ff3, ; 612: System.Memory.dll => 160
	i64 u0xedc8e4ca71a02a8b, ; 613: Xamarin.AndroidX.Navigation.Runtime.dll => 113
	i64 u0xee81f5b3f1c4f83b, ; 614: System.Threading.ThreadPool => 209
	i64 u0xeeb7ebb80150501b, ; 615: lib_Xamarin.AndroidX.Collection.Jvm.dll.so => 99
	i64 u0xeefc635595ef57f0, ; 616: System.Security.Cryptography.Cng => 194
	i64 u0xef03b1b5a04e9709, ; 617: System.Text.Encoding.CodePages.dll => 201
	i64 u0xef72742e1bcca27a, ; 618: Microsoft.Maui.Essentials.dll => 79
	i64 u0xefd0396433f04886, ; 619: pt-BR/Microsoft.Data.SqlClient.resources => 6
	i64 u0xefd1e0c4e5c9b371, ; 620: System.Resources.ResourceManager.dll => 179
	i64 u0xefec0b7fdc57ec42, ; 621: Xamarin.AndroidX.Activity => 94
	i64 u0xf00c29406ea45e19, ; 622: es/Microsoft.Maui.Controls.resources.dll => 16
	i64 u0xf09e47b6ae914f6e, ; 623: System.Net.NameResolution => 162
	i64 u0xf0ac2b489fed2e35, ; 624: lib_System.Diagnostics.Debug.dll.so => 141
	i64 u0xf0de2537ee19c6ca, ; 625: lib_System.Net.WebHeaderCollection.dll.so => 169
	i64 u0xf11b621fc87b983f, ; 626: Microsoft.Maui.Controls.Xaml.dll => 77
	i64 u0xf1c4b4005493d871, ; 627: System.Formats.Asn1.dll => 150
	i64 u0xf238bd79489d3a96, ; 628: lib-nl-Microsoft.Maui.Controls.resources.dll.so => 29
	i64 u0xf37221fda4ef8830, ; 629: lib_Xamarin.Google.Android.Material.dll.so => 120
	i64 u0xf3ddfe05336abf29, ; 630: System => 217
	i64 u0xf408654b2a135055, ; 631: System.Reflection.Emit.ILGeneration.dll => 176
	i64 u0xf4103170a1de5bd0, ; 632: System.Linq.Queryable.dll => 158
	i64 u0xf4c1dd70a5496a17, ; 633: System.IO.Compression => 152
	i64 u0xf518f63ead11fcd1, ; 634: System.Threading.Tasks => 207
	i64 u0xf5e59d7ac34b50aa, ; 635: Microsoft.IdentityModel.Protocols.dll => 73
	i64 u0xf5fc7602fe27b333, ; 636: System.Net.WebHeaderCollection => 169
	i64 u0xf6077741019d7428, ; 637: Xamarin.AndroidX.CoordinatorLayout => 100
	i64 u0xf61ade9836ad4692, ; 638: Microsoft.IdentityModel.Tokens.dll => 75
	i64 u0xf6c0e7d55a7a4e4f, ; 639: Microsoft.IdentityModel.JsonWebTokens => 71
	i64 u0xf77b20923f07c667, ; 640: de/Microsoft.Maui.Controls.resources.dll => 14
	i64 u0xf7be8a85d06b4b64, ; 641: ru/Microsoft.Data.SqlClient.resources.dll => 7
	i64 u0xf7e2cac4c45067b3, ; 642: lib_System.Numerics.Vectors.dll.so => 170
	i64 u0xf7e74930e0e3d214, ; 643: zh-HK/Microsoft.Maui.Controls.resources.dll => 41
	i64 u0xf83775f330791063, ; 644: ja/Microsoft.Data.SqlClient.resources.dll => 4
	i64 u0xf84773b5c81e3cef, ; 645: lib-uk-Microsoft.Maui.Controls.resources.dll.so => 39
	i64 u0xf8aac5ea82de1348, ; 646: System.Linq.Queryable => 158
	i64 u0xf8b77539b362d3ba, ; 647: lib_System.Reflection.Primitives.dll.so => 178
	i64 u0xf8cd217ba1bbfdc8, ; 648: lib-zh-Hant-Microsoft.Data.SqlClient.resources.dll.so => 9
	i64 u0xf8e045dc345b2ea3, ; 649: lib_Xamarin.AndroidX.RecyclerView.dll.so => 115
	i64 u0xf915dc29808193a1, ; 650: System.Web.HttpUtility.dll => 212
	i64 u0xf96c777a2a0686f4, ; 651: hi/Microsoft.Maui.Controls.resources.dll => 20
	i64 u0xf9be54c8bcf8ff3b, ; 652: System.Security.AccessControl.dll => 191
	i64 u0xf9eec5bb3a6aedc6, ; 653: Microsoft.Extensions.Options => 66
	i64 u0xfa2fdb27e8a2c8e8, ; 654: System.ComponentModel.EventBasedAsync => 135
	i64 u0xfa3f278f288b0e84, ; 655: lib_System.Net.Security.dll.so => 166
	i64 u0xfa5ed7226d978949, ; 656: lib-ar-Microsoft.Maui.Controls.resources.dll.so => 10
	i64 u0xfa645d91e9fc4cba, ; 657: System.Threading.Thread => 208
	i64 u0xfbad3e4ce4b98145, ; 658: System.Security.Cryptography.X509Certificates => 198
	i64 u0xfbf0a31c9fc34bc4, ; 659: lib_System.Net.Http.dll.so => 161
	i64 u0xfc6b7527cc280b3f, ; 660: lib_System.Runtime.Serialization.Formatters.dll.so => 186
	i64 u0xfc719aec26adf9d9, ; 661: Xamarin.AndroidX.Navigation.Fragment.dll => 112
	i64 u0xfcd5b90cf101e36b, ; 662: System.Data.SqlClient.dll => 86
	i64 u0xfd22f00870e40ae0, ; 663: lib_Xamarin.AndroidX.DrawerLayout.dll.so => 104
	i64 u0xfd49b3c1a76e2748, ; 664: System.Runtime.InteropServices.RuntimeInformation => 181
	i64 u0xfd536c702f64dc47, ; 665: System.Text.Encoding.Extensions => 202
	i64 u0xfd583f7657b6a1cb, ; 666: Xamarin.AndroidX.Fragment => 105
	i64 u0xfeae9952cf03b8cb, ; 667: tr/Microsoft.Maui.Controls.resources => 38
	i64 u0xfff40914e0b38d3d ; 668: Azure.Identity.dll => 46
], align 16

@assembly_image_cache_indices = dso_local local_unnamed_addr constant [669 x i32] [
	i32 0, i32 117, i32 134, i32 113, i32 5, i32 59, i32 221, i32 95,
	i32 128, i32 172, i32 34, i32 12, i32 40, i32 70, i32 164, i32 115,
	i32 133, i32 78, i32 180, i32 9, i32 41, i32 213, i32 99, i32 53,
	i32 34, i32 131, i32 3, i32 45, i32 178, i32 104, i32 134, i32 66,
	i32 131, i32 89, i32 199, i32 58, i32 205, i32 54, i32 35, i32 123,
	i32 118, i32 31, i32 222, i32 79, i32 52, i32 162, i32 4, i32 52,
	i32 103, i32 1, i32 151, i32 197, i32 177, i32 49, i32 115, i32 74,
	i32 97, i32 18, i32 220, i32 19, i32 74, i32 63, i32 127, i32 87,
	i32 0, i32 156, i32 193, i32 218, i32 191, i32 22, i32 203, i32 123,
	i32 168, i32 28, i32 192, i32 46, i32 129, i32 217, i32 37, i32 221,
	i32 207, i32 153, i32 48, i32 144, i32 114, i32 26, i32 66, i32 216,
	i32 168, i32 151, i32 92, i32 143, i32 190, i32 84, i32 176, i32 37,
	i32 156, i32 49, i32 208, i32 55, i32 179, i32 125, i32 139, i32 101,
	i32 188, i32 18, i32 121, i32 67, i32 88, i32 23, i32 21, i32 220,
	i32 141, i32 164, i32 125, i32 39, i32 163, i32 146, i32 17, i32 204,
	i32 88, i32 150, i32 43, i32 30, i32 201, i32 177, i32 174, i32 6,
	i32 210, i32 36, i32 93, i32 15, i32 143, i32 214, i32 81, i32 147,
	i32 58, i32 105, i32 70, i32 44, i32 98, i32 148, i32 18, i32 214,
	i32 130, i32 16, i32 167, i32 81, i32 78, i32 12, i32 76, i32 134,
	i32 119, i32 60, i32 183, i32 177, i32 178, i32 130, i32 180, i32 103,
	i32 162, i32 73, i32 85, i32 118, i32 11, i32 197, i32 82, i32 202,
	i32 75, i32 198, i32 54, i32 121, i32 97, i32 86, i32 209, i32 213,
	i32 101, i32 48, i32 3, i32 69, i32 206, i32 82, i32 111, i32 46,
	i32 83, i32 85, i32 96, i32 179, i32 218, i32 222, i32 30, i32 126,
	i32 188, i32 121, i32 56, i32 88, i32 146, i32 83, i32 34, i32 213,
	i32 89, i32 32, i32 92, i32 171, i32 114, i32 153, i32 93, i32 84,
	i32 57, i32 110, i32 163, i32 157, i32 175, i32 84, i32 184, i32 24,
	i32 110, i32 221, i32 201, i32 205, i32 11, i32 75, i32 3, i32 76,
	i32 108, i32 4, i32 149, i32 164, i32 140, i32 8, i32 101, i32 80,
	i32 35, i32 194, i32 163, i32 181, i32 41, i32 192, i32 190, i32 106,
	i32 132, i32 189, i32 173, i32 219, i32 142, i32 25, i32 62, i32 126,
	i32 89, i32 100, i32 210, i32 138, i32 172, i32 13, i32 51, i32 86,
	i32 64, i32 169, i32 6, i32 182, i32 99, i32 132, i32 203, i32 136,
	i32 195, i32 214, i32 140, i32 15, i32 180, i32 62, i32 122, i32 160,
	i32 50, i32 77, i32 14, i32 184, i32 219, i32 130, i32 120, i32 193,
	i32 53, i32 124, i32 76, i32 185, i32 139, i32 108, i32 102, i32 13,
	i32 148, i32 150, i32 19, i32 182, i32 28, i32 91, i32 80, i32 135,
	i32 67, i32 102, i32 67, i32 112, i32 78, i32 12, i32 154, i32 38,
	i32 28, i32 24, i32 136, i32 196, i32 21, i32 160, i32 60, i32 116,
	i32 185, i32 50, i32 194, i32 27, i32 37, i32 92, i32 125, i32 105,
	i32 8, i32 17, i32 71, i32 137, i32 35, i32 14, i32 168, i32 27,
	i32 170, i32 133, i32 47, i32 147, i32 192, i32 171, i32 138, i32 118,
	i32 61, i32 68, i32 107, i32 217, i32 43, i32 124, i32 95, i32 98,
	i32 149, i32 39, i32 70, i32 42, i32 156, i32 187, i32 57, i32 5,
	i32 43, i32 60, i32 189, i32 56, i32 208, i32 151, i32 79, i32 122,
	i32 218, i32 136, i32 181, i32 58, i32 110, i32 142, i32 200, i32 143,
	i32 19, i32 50, i32 45, i32 200, i32 102, i32 210, i32 129, i32 68,
	i32 87, i32 111, i32 20, i32 33, i32 32, i32 31, i32 146, i32 44,
	i32 152, i32 206, i32 108, i32 77, i32 103, i32 93, i32 159, i32 11,
	i32 1, i32 27, i32 152, i32 72, i32 74, i32 72, i32 16, i32 69,
	i32 23, i32 80, i32 138, i32 129, i32 157, i32 51, i32 113, i32 26,
	i32 73, i32 211, i32 198, i32 94, i32 61, i32 29, i32 141, i32 111,
	i32 107, i32 68, i32 199, i32 120, i32 114, i32 155, i32 26, i32 140,
	i32 186, i32 154, i32 195, i32 170, i32 199, i32 216, i32 82, i32 116,
	i32 104, i32 54, i32 127, i32 106, i32 22, i32 71, i32 65, i32 175,
	i32 161, i32 144, i32 63, i32 15, i32 157, i32 185, i32 107, i32 48,
	i32 200, i32 215, i32 33, i32 205, i32 29, i32 212, i32 137, i32 166,
	i32 135, i32 220, i32 171, i32 109, i32 36, i32 147, i32 191, i32 204,
	i32 195, i32 13, i32 91, i32 98, i32 20, i32 10, i32 155, i32 153,
	i32 207, i32 65, i32 209, i32 148, i32 36, i32 219, i32 126, i32 32,
	i32 25, i32 215, i32 133, i32 49, i32 167, i32 69, i32 186, i32 8,
	i32 90, i32 2, i32 161, i32 2, i32 119, i32 7, i32 97, i32 183,
	i32 128, i32 96, i32 10, i32 139, i32 183, i32 159, i32 55, i32 94,
	i32 47, i32 128, i32 72, i32 25, i32 154, i32 119, i32 53, i32 189,
	i32 187, i32 55, i32 109, i32 59, i32 137, i32 165, i32 112, i32 175,
	i32 9, i32 173, i32 193, i32 124, i32 202, i32 212, i32 132, i32 145,
	i32 59, i32 38, i32 30, i32 33, i32 57, i32 44, i32 52, i32 204,
	i32 122, i32 123, i32 42, i32 165, i32 90, i32 117, i32 45, i32 81,
	i32 142, i32 174, i32 187, i32 145, i32 106, i32 91, i32 0, i32 182,
	i32 196, i32 96, i32 173, i32 64, i32 172, i32 83, i32 24, i32 56,
	i32 63, i32 109, i32 7, i32 116, i32 117, i32 61, i32 216, i32 65,
	i32 47, i32 62, i32 131, i32 40, i32 176, i32 40, i32 211, i32 159,
	i32 211, i32 158, i32 64, i32 190, i32 203, i32 42, i32 215, i32 206,
	i32 21, i32 51, i32 95, i32 100, i32 5, i32 1, i32 197, i32 2,
	i32 155, i32 184, i32 174, i32 166, i32 145, i32 165, i32 23, i32 127,
	i32 31, i32 196, i32 22, i32 17, i32 188, i32 222, i32 85, i32 90,
	i32 144, i32 149, i32 87, i32 167, i32 160, i32 113, i32 209, i32 99,
	i32 194, i32 201, i32 79, i32 6, i32 179, i32 94, i32 16, i32 162,
	i32 141, i32 169, i32 77, i32 150, i32 29, i32 120, i32 217, i32 176,
	i32 158, i32 152, i32 207, i32 73, i32 169, i32 100, i32 75, i32 71,
	i32 14, i32 7, i32 170, i32 41, i32 4, i32 39, i32 158, i32 178,
	i32 9, i32 115, i32 212, i32 20, i32 191, i32 66, i32 135, i32 166,
	i32 10, i32 208, i32 198, i32 161, i32 186, i32 112, i32 86, i32 104,
	i32 181, i32 202, i32 105, i32 38, i32 46
], align 16

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
@.str.0 = private unnamed_addr constant [40 x i8] c"get_function_pointer MUST be specified\0A\00", align 16

;MarshalMethodName
@.MarshalMethodName.0_name = private unnamed_addr constant [1 x i8] c"\00", align 1

; External functions

; Function attributes: noreturn "no-trapping-math"="true" nounwind "stack-protector-buffer-size"="8"
declare void @abort() local_unnamed_addr #2

; Function attributes: nofree nounwind
declare noundef i32 @puts(ptr noundef) local_unnamed_addr #1
attributes #0 = { memory(write, argmem: none, inaccessiblemem: none) "min-legal-vector-width"="0" mustprogress nofree norecurse nosync "no-trapping-math"="true" nounwind "stack-protector-buffer-size"="8" "target-cpu"="x86-64" "target-features"="+crc32,+cx16,+cx8,+fxsr,+mmx,+popcnt,+sse,+sse2,+sse3,+sse4.1,+sse4.2,+ssse3,+x87" "tune-cpu"="generic" uwtable willreturn }
attributes #1 = { nofree nounwind }
attributes #2 = { noreturn "no-trapping-math"="true" nounwind "stack-protector-buffer-size"="8" "target-cpu"="x86-64" "target-features"="+crc32,+cx16,+cx8,+fxsr,+mmx,+popcnt,+sse,+sse2,+sse3,+sse4.1,+sse4.2,+ssse3,+x87" "tune-cpu"="generic" }

; Metadata
!llvm.module.flags = !{!0, !1}
!0 = !{i32 1, !"wchar_size", i32 4}
!1 = !{i32 7, !"PIC Level", i32 2}
!llvm.ident = !{!2}
!2 = !{!".NET for Android remotes/origin/release/9.0.1xx-rc2 @ 150245588b8014834bdf6ff8b7b282f86d8e2ea2"}
!3 = !{!4, !4, i64 0}
!4 = !{!"any pointer", !5, i64 0}
!5 = !{!"omnipotent char", !6, i64 0}
!6 = !{!"Simple C++ TBAA"}
