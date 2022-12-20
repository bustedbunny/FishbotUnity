using System;
using System.Collections.Generic;
using Newtonsoft.Json;

// ReSharper disable InconsistentNaming

namespace Fishbot.Model.VersionChecking.GithubReleaseData
{
    public class Asset
    {
        [JsonProperty("url")] public string url;

        [JsonProperty("id")] public int id;

        [JsonProperty("node_id")] public string node_id;

        [JsonProperty("name")] public string name;

        [JsonProperty("label")] public object label;

        [JsonProperty("content_type")] public string content_type;

        [JsonProperty("state")] public string state;

        [JsonProperty("size")] public int size;

        [JsonProperty("download_count")] public int download_count;

        [JsonProperty("created_at")] public DateTime created_at;

        [JsonProperty("updated_at")] public DateTime updated_at;

        [JsonProperty("browser_download_url")] public string browser_download_url;
    }


    public class Root
    {
        [JsonProperty("url")] public string url;

        [JsonProperty("assets_url")] public string assets_url;

        [JsonProperty("upload_url")] public string upload_url;

        [JsonProperty("html_url")] public string html_url;

        [JsonProperty("id")] public int id;

        [JsonProperty("node_id")] public string node_id;

        [JsonProperty("tag_name")] public string tag_name;

        [JsonProperty("target_commitish")] public string target_commitish;

        [JsonProperty("name")] public string name;

        [JsonProperty("draft")] public bool draft;

        [JsonProperty("prerelease")] public bool prerelease;

        [JsonProperty("created_at")] public DateTime created_at;

        [JsonProperty("published_at")] public DateTime published_at;

        [JsonProperty("assets")] public List<Asset> assets;

        [JsonProperty("tarball_url")] public string tarball_url;

        [JsonProperty("zipball_url")] public string zipball_url;

        [JsonProperty("body")] public string body;
    }
}