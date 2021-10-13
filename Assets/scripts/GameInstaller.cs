using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller
{
    [SerializeField] private BPManager _bpManager;
    
    public override void InstallBindings()
    {
        CreateSingletons();
    }

    private void CreateSingletons()
    {
        Container.Bind<BPManager>().FromInstance(_bpManager).AsSingle().NonLazy();
    }
}