public enum BuffType
{
    Buff,
    Debuff
}

public enum BuffStackType
{
    RefreshDuration, // buff mới reset thời gian
    Stack,           // cộng dồn
    Ignore           // đã có thì bỏ qua
}
