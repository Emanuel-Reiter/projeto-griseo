public readonly struct AudioHandle
{
    private readonly AudioPool Pool;
    private readonly object VoiceRef;

    internal AudioHandle(AudioPool pool, object voiceRef)
    {
        this.Pool = pool;
        this.VoiceRef = voiceRef;
    }

    public bool IsValid => Pool != null && VoiceRef != null;

    public void Stop()
    {
        if (!IsValid) return;
        Pool.StopVoice(VoiceRef as AudioPool.PooledVoice);
    }
}
