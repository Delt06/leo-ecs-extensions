# LeoECS (Lite) Extensions

## Dependencies
- LeoEcs Lite: [official](https://github.com/Leopotam/ecslite) or [community version](https://github.com/LeoECSCommunity/ecslite)
- LeoEcs Lite Unity Integration: [official](https://github.com/Leopotam/ecslite-unityeditor) or [community version](https://github.com/LeoECSCommunity/ecslite-unityeditor)
- LeoEcs Lite DI (*Optional*): [official](https://github.com/Leopotam/ecslite-di) or [community version](https://github.com/LeoECSCommunity/ecslite-di)
- [DI Framework](https://github.com/Delt06/di-framework)

## Installation
Before proceeding, the dependencies have to be installed.
### Option 1
- Open Package Manager through Window/Package Manager
- Click "+" and choose "Add package from git URL..."
- Insert the URL: https://github.com/Delt06/leo-ecs-extensions.git?path=Packages/com.deltation.leo-ecs-extensions

### Option 2
Add the following line to `Packages/manifest.json`:
```
"com.deltation.leo-ecs-extensions": "https://github.com/Delt06/leo-ecs-extensions.git?path=Packages/com.deltation.leo-ecs-extensions",
```

### Assembly Definitions

If want to use the package in an assembly definition, make sure to include references to the following assemblies:
- `Leopotam.EcsLite`
- `DELTation.LeoEcsExtensions`
- `DELTation.LeoEcsExtensions.Composition.Di` (only if you use `EcsEntryPoint`)
