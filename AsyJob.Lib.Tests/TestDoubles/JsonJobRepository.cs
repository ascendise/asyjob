﻿using AsyJob.Lib.Jobs;
using AsyJob.Lib.Jobs.Factory;
using AsyJob.Lib.Runner;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AsyJob.Lib.Tests.TestDoubles
{
    internal class JsonJobRepository : IJobRepository
    {
        private List<string> _jobs = [];
        private static readonly JsonSerializerSettings s_jsonSettings = new()
        {
            TypeNameHandling = TypeNameHandling.All,
            ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
            ContractResolver = CreateContractResolver(),
        };

        public static DefaultContractResolver CreateContractResolver()
        {
            var contractResolver = new DefaultContractResolver();
            //FIXME: Use new method
#pragma warning disable CS0618 // Type or member is obsolete
            contractResolver.DefaultMembersSearchFlags |= BindingFlags.NonPublic;
#pragma warning restore CS0618 // Type or member is obsolete
            return contractResolver;
        }

        public Task<IEnumerable<Job>> FetchAllJobs()
        {
            var jobs = GetJobs();
            return Task.FromResult(jobs.AsEnumerable());
        }

        private List<Job> GetJobs()
        {
            return _jobs.Select(j => JsonConvert.DeserializeObject<Job>(j, s_jsonSettings)
                ?? throw new TestException("Malformed json")).ToList();
        }

        public async Task<Job?> FetchJob(string id)
            => (await FetchAllJobs()).Where(j => j.Id == id).FirstOrDefault();

        public Task SaveJob(Job job)
        {
            var json = JsonConvert.SerializeObject(job, s_jsonSettings);
            _jobs.Add(json);
            return Task.CompletedTask;
        }

        public Task<Job> UpdateJob(Job job)
        {
            var jobs = GetJobs();
            var jobToUpdate = jobs.FirstOrDefault(j => j.Id == job.Id)
                ?? throw new KeyNotFoundException();
            jobToUpdate.Update(job);
            SaveCollectionToJson(jobs);
            return Task.FromResult(jobToUpdate);
        }

        private void SaveCollectionToJson(IEnumerable<Job> jobs)
        {
            _jobs = jobs.Select(j => JsonConvert.SerializeObject(j, s_jsonSettings)).ToList();
        }

        public async Task DeleteJob(string jobId)
        {
            var jobs = await FetchAllJobs();
            jobs = jobs.Where(j => j.Id != jobId);
            SaveCollectionToJson(jobs);
        }
    }
}
