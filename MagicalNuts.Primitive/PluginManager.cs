using System;
using System.Collections.Generic;
using System.Reflection;

namespace MagicalNuts.Primitive
{
	/// <summary>
	/// プラグイン管理を表します。
	/// </summary>
	public class PluginManager<T>
	{
		/// <summary>
		/// プラグイン情報を表します。
		/// </summary>
		private class PluginInfo
		{
			/// <summary>
			/// プラグインが属すアセンブリを取得します。
			/// </summary>
			public Assembly Assembly { get; private set; }

			/// <summary>
			/// プラグインのクラス名を取得します。
			/// </summary>
			public string ClassName { get; private set; }

			/// <summary>
			/// PluginInfoの新しいインスタンスを初期化します。
			/// </summary>
			/// <param name="assembly">アセンブリ</param>
			/// <param name="cn">クラス名</param>
			public PluginInfo(Assembly assembly, string cn)
			{
				Assembly = assembly;
				ClassName = cn;
			}
		}

		/// <summary>
		/// プラグイン情報のリスト
		/// </summary>
		private List<PluginInfo> PluginInfos = null;

		/// <summary>
		/// PluginManagerクラスの新しいインスタンスを初期化します。
		/// </summary>
		/// <param name="plugin_path">プラグインのパス</param>
		public PluginManager(string plugin_path = null)
		{
			// プラグイン情報収集
			PluginInfos = new List<PluginInfo>();

			// 自アプリ内
			foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
			{
				PluginInfos.AddRange(GetPluginInfos(assembly));
			}

			// プラグイン
			if (plugin_path != null)
			{
				if (System.IO.Directory.Exists(plugin_path))
				{
					string[] dlls = System.IO.Directory.GetFiles(plugin_path, "*.dll");
					foreach (string dll in dlls)
					{
						PluginInfos.AddRange(GetPluginInfos(Assembly.LoadFrom(dll)));
					}
				}
			}
		}

		/// <summary>
		/// プラグイン情報のリストを取得します。
		/// </summary>
		/// <param name="assembly">アセンブリ</param>
		/// <returns>プラグイン情報のリスト</returns>
		private List<PluginInfo> GetPluginInfos(Assembly assembly)
		{
			List<PluginInfo> pluginInfos = new List<PluginInfo>();
			foreach (Type type in assembly.GetTypes())
			{
				// 除外ケース
				if (IsExclude(type)) continue;

				// クラス、公開、抽象クラスでない、Tを継承している、が条件
				if (type.IsClass && type.IsPublic && !type.IsAbstract
					&& (type.GetInterface(typeof(T).FullName) != null || type.IsSubclassOf(typeof(T))))
				{
					pluginInfos.Add(new PluginInfo(assembly, type.FullName));
				}
			}
			return pluginInfos;
		}

		/// <summary>
		/// 除外する型かどうか判定します。
		/// </summary>
		/// <param name="type">型</param>
		/// <returns>除外する型かどうか</returns>
		protected virtual bool IsExclude(Type type)
		{
			return false;
		}

		/// <summary>
		/// プラグインのリストを取得します。
		/// </summary>
		public List<T> Plugins
		{
			get
			{
				List<T> plugins = new List<T>();
				foreach (PluginInfo pi in PluginInfos)
				{
					plugins.Add((T)pi.Assembly.CreateInstance(pi.ClassName));
				}
				return plugins;
			}
		}
	}
}
