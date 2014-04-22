using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

namespace Configuration.DkimSigner.GitHub
{

    /*
    {
    "url": "https://api.github.com/repos/octocat/Hello-World/releases/1",
    "html_url": "https://github.com/octocat/Hello-World/releases/v1.0.0",
    "assets_url": "https://api.github.com/repos/octocat/Hello-World/releases/1/assets",
    "upload_url": "https://uploads.github.com/repos/octocat/Hello-World/releases/1/assets{?name}",
    "tarball_url": "https://api.github.com/repos/octocat/Hello-World/tarball/v1.0.0",
    "zipball_url": "https://api.github.com/repos/octocat/Hello-World/zipball/v1.0.0",
    "id": 1,
    "tag_name": "v1.0.0",
    "target_commitish": "master",
    "name": "v1.0.0",
    "body": "Description of the release",
    "draft": false,
    "prerelease": false,
    "created_at": "2013-02-27T19:35:32Z",
    "published_at": "2013-02-27T19:35:32Z",
    "author": {
    },
    "assets": [
    ]
  }
     * */
    [DataContract]
    public class Release
    {
        [DataMember(Name = "url")]
        public string Url { get; set; }
        [DataMember(Name = "html_url")]
        public string HtmlUrl { get; set; }
        [DataMember(Name = "assets_url")]
        public string AssetsUrl { get; set; }
        [DataMember(Name = "tarball_url")]
        public string TarballUrl { get; set; }
        [DataMember(Name = "zipball_url")]
        public string ZipballUrl { get; set; }
        [DataMember(Name = "id")]
        public int Id { get; set; }
        [DataMember(Name = "tag_name")]
        public string TagName { get; set; }
        [DataMember(Name = "target_commitish")]
        public string TargetCommitish { get; set; }
        [DataMember(Name = "name")]
        public string Name { get; set; }
        [DataMember(Name = "body")]
        public string Body { get; set; }
        [DataMember(Name = "draft")]
        public bool Draft { get; set; }
        [DataMember(Name = "prerelease")]
        public bool Prerelease { get; set; }
        [DataMember(Name = "created_at")]
        public string CreatedAt { get; set; }
        [DataMember(Name = "published_at")]
        public string PublishedAt { get; set; }
        [DataMember(Name = "author")]
        public User Author { get; set; }
        [DataMember(Name = "assets")]
        public Asset[] Resources { get; set; }

        public Version Version { get; set; }
    }

    /*
    {
      "login": "octocat",
      "id": 1,
      "avatar_url": "https://github.com/images/error/octocat_happy.gif",
      "gravatar_id": "somehexcode",
      "url": "https://api.github.com/users/octocat",
      "html_url": "https://github.com/octocat",
      "followers_url": "https://api.github.com/users/octocat/followers",
      "following_url": "https://api.github.com/users/octocat/following{/other_user}",
      "gists_url": "https://api.github.com/users/octocat/gists{/gist_id}",
      "starred_url": "https://api.github.com/users/octocat/starred{/owner}{/repo}",
      "subscriptions_url": "https://api.github.com/users/octocat/subscriptions",
      "organizations_url": "https://api.github.com/users/octocat/orgs",
      "repos_url": "https://api.github.com/users/octocat/repos",
      "events_url": "https://api.github.com/users/octocat/events{/privacy}",
      "received_events_url": "https://api.github.com/users/octocat/received_events",
      "type": "User",
      "site_admin": false
    }
     * */
    [DataContract]
    public class User
    {
        [DataMember(Name = "login")]
        public string Login { get; set; }
        [DataMember(Name = "id")]
        public int Id { get; set; }
        [DataMember(Name = "avatar_url")]
        public string AvatarUrl { get; set; }
        [DataMember(Name = "gravatar_id")]
        public string GravatarId { get; set; }
        [DataMember(Name = "url")]
        public string Url { get; set; }
        [DataMember(Name = "followers_url")]
        public string FollowersUrl { get; set; }
        [DataMember(Name = "following_url")]
        public string FollowingUrl { get; set; }
        [DataMember(Name = "gists_url")]
        public string GistsUrl { get; set; }
        [DataMember(Name = "starred_url")]
        public string StarredUrl { get; set; }
        [DataMember(Name = "subscriptions_url")]
        public string SubscriptionsUrl { get; set; }
        [DataMember(Name = "organizations_url")]
        public string OrganizationsUrl { get; set; }
        [DataMember(Name = "repos_url")]
        public string ReposUrl { get; set; }
        [DataMember(Name = "events_url")]
        public string EventsUrl { get; set; }
        [DataMember(Name = "received_events_url")]
        public string ReceivedEventsUrl { get; set; }
        [DataMember(Name = "type")]
        public string Type { get; set; }
        [DataMember(Name = "site_admin")]
        public bool SiteAdmin { get; set; }
    }

    /*
    {
        "url": "https://api.github.com/repos/octocat/Hello-World/releases/assets/1",
        "id": 1,
        "name": "example.zip",
        "label": "short description",
        "state": "uploaded",
        "content_type": "application/zip",
        "size": 1024,
        "download_count": 42,
        "created_at": "2013-02-27T19:35:32Z",
        "updated_at": "2013-02-27T19:35:32Z",
        "uploader": {
        }
      }
     * */
    [DataContract]
    public class Asset
    {
        [DataMember(Name = "url")]
        public string Url { get; set; }
        [DataMember(Name = "id")]
        public int Id { get; set; }
        [DataMember(Name = "name")]
        public string Name { get; set; }
        [DataMember(Name = "label")]
        public string Label { get; set; }
        [DataMember(Name = "state")]
        public string State { get; set; }
        [DataMember(Name = "content_type")]
        public string ContentType { get; set; }
        [DataMember(Name = "size")]
        public int Size { get; set; }
        [DataMember(Name = "download_count")]
        public int DownloadCount { get; set; }
        [DataMember(Name = "created_at")]
        public string CreatedAt { get; set; }
        [DataMember(Name = "updated_at")]
        public string UpdatedAt { get; set; }
        [DataMember(Name = "uploader")]
        public User Uploader { get; set; }
    }

}
