# CupkekGames PrefabLoader

Prefab loading abstraction. Extracted from `com.cupkekgames.core`.

## What's inside

**Runtime** (`CupkekGames.PrefabLoaders.asmdef`)

- `IPrefabLoaderBase` — non-generic marker interface
- `IPrefabLoader<TKey, TValue>` — generic loader interface (extends `IKeyValueDatabase<TKey, TValue>`)
- `PrefabLoader<TKey>` — abstract `MonoBehaviour` loader for `GameObject` keyed by TKey
- `PrefabLoaderReportDestroy` — runtime helper for instance lifecycle reporting
- `PrefabLoaderString` — string-keyed concrete loader

**Editor** (`CupkekGames.PrefabLoaders.Editor.asmdef`)

- `PrefabLoaderClassicEditor` — custom inspector for generic `PrefabLoader<TKey>`
- `PrefabLoaderStringEditor` — inspector for `PrefabLoaderString`

## Dependencies

- `com.cupkekgames.keyvaluedatabases`
