using System;
using System.Collections.Generic;

namespace RapidPhotoWatcher.Avalonia.Services
{
    /// <summary>
    /// シンプルなサービスコンテナ実装
    /// </summary>
    public class ServiceContainer
    {
        private static ServiceContainer? _instance;
        private readonly Dictionary<Type, object> _services = new();

        public static ServiceContainer Instance => _instance ??= new ServiceContainer();

        private ServiceContainer() { }

        /// <summary>
        /// サービスを登録
        /// </summary>
        public void RegisterSingleton<TInterface, TImplementation>(TImplementation implementation)
            where TImplementation : class, TInterface
        {
            _services[typeof(TInterface)] = implementation;
        }

        /// <summary>
        /// サービスを取得
        /// </summary>
        public T GetService<T>()
        {
            if (_services.TryGetValue(typeof(T), out var service))
            {
                return (T)service;
            }
            
            throw new InvalidOperationException($"Service of type {typeof(T).Name} is not registered.");
        }

        /// <summary>
        /// サービスの取得を試行
        /// </summary>
        public bool TryGetService<T>(out T? service)
        {
            if (_services.TryGetValue(typeof(T), out var obj))
            {
                service = (T)obj;
                return true;
            }
            
            service = default;
            return false;
        }

        /// <summary>
        /// サービスコンテナを初期化
        /// </summary>
        public static void Initialize()
        {
            var container = Instance;
            
            // 必要なサービスを登録
            container.RegisterSingleton<IDialogService, AvaloniaDialogService>(new AvaloniaDialogService());
        }
    }
}