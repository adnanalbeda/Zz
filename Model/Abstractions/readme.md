# The unspoken rule of generic

It's not really a rule, rather than a contract I made up.

Only use generic for last type in hierarchy, like `ITrackCreate<MoneyFX>`, 
so when creating custom extensions for them, you can select them as a generic `T`,
while you're almost sure no weird projection or select is made into the type.