using System;

[Serializable]
public class HealthCheckResponse
{
    public bool success;
    public string message;
    public HealthCheckData data;
}

[Serializable]
public class HealthCheckData
{
    public string apiStatus;
    public string databaseStatus;
    public string framework;
    public string database;
}