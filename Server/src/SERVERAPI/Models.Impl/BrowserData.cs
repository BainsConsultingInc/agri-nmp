﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SERVERAPI.Models.Impl
{
    public partial class BrowserData
    {
        private readonly IHttpContextAccessor _ctx;
        private readonly StaticData _sd;
        public string BrowserName { get; }
        public string BrowserVersion { get; }
        public bool BrowserValid { get; }
        public bool BrowserOutofdate { get; }
        public string BrowserAgent { get; }

        public BrowserData(IHttpContextAccessor ctx, StaticData sd)
        {
            _ctx = ctx;
            _sd = sd;

            try
            {
                string fake = "Mozilla/5.0 (Macintosh; Itenl Mac OS X 10.12.3 AppleWebKit/602.4.8 (KHTML, like Gecko) Version/10.0.3 Safari/602.4.8";

                UserAgent.UserAgent ua = new UserAgent.UserAgent(fake);
                //UserAgent.UserAgent ua = new UserAgent.UserAgent(_ctx.HttpContext.Request.Headers["User-Agent"]);
                BrowserName = ua.Browser.Name;
                BrowserVersion = ua.Browser.Version;
                BrowserAgent = _ctx.HttpContext.Request.Headers["User-Agent"].ToString();

                Models.StaticData.Browsers ab = _sd.GetAllowableBrowsers();
                int indx = ab.known.FindIndex(r => r.name == BrowserName);
                if(indx < 0)
                {
                    BrowserValid = false;
                    BrowserName = "Unknown";
                }
                else
                {
                    BrowserValid = true;
                    var minVer = Version.Parse(ab.known[indx].minVersion);
                    var thisVer = Version.Parse(BrowserVersion);
                    if(thisVer < minVer)
                    {
                        BrowserOutofdate = true;
                    }
                    else
                    {
                        BrowserOutofdate = false;
                    }
                }
            }
            catch (Exception ex)
            {
                BrowserName = "Unknown";
                throw new Exception("Could not retrieve browser type.!");
            }
        }
    }
}
