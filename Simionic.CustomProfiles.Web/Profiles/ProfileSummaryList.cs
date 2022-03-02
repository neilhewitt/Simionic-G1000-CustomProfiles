using Simionic.CustomProfiles.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Simionic.CustomProfiles.Web
{
    public class ProfileSummaryList : List<ProfileSummary>
    {
        public ProfileSummaryList FilterByPublished() => new ProfileSummaryList(this.Where(x => x.IsPublished));
        public ProfileSummaryList FilterByType(AircraftType type) => new ProfileSummaryList(this.Where(x => x.AircraftType == type));
        public ProfileSummaryList FilterByEngineCount(int engines) => new ProfileSummaryList(this.Where(x => x.Engines == engines));
        public ProfileSummaryList FilterByOwner(string ownerId) => new ProfileSummaryList(this.Where(x => x.Owner?.Id is not null && x.Owner.Id == ownerId));

        public ProfileSummaryList FilterBySearch(string searchTerms)
        {
            if (String.IsNullOrWhiteSpace(searchTerms)) return this;

            IEnumerable<ProfileSummary> output = this;
            string[] terms = searchTerms.Split(' ', ',');
            foreach (string term in terms)
            {
                output = output.Where(
                    x => x.Name is not null && x.Name.ToLower().Contains(term.ToLower()) ||
                    x.Owner.Name is not null && x.Owner.Name.ToLower().Contains(term.ToLower()) ||
                    x.AircraftType.ToString().ToLower().Contains(term.ToLower())
                    );
            }

            return new ProfileSummaryList(output);
        }

        public ProfileSummaryList Filter(PublishedStatus published, AircraftType? type, int? engines, string ownerId, bool ownerOnly, string searchTerms)
        {
            ProfileSummaryList output = this;
            output = published switch
            {
                PublishedStatus.Unpublished => new ProfileSummaryList(this.Where(x => !x.IsPublished)),
                PublishedStatus.PublishedOwner => new ProfileSummaryList(this.Where(x => x.IsPublished || x.Owner.Id == ownerId)),
                PublishedStatus.UnpublishedOwner => new ProfileSummaryList(this.Where(x => !x.IsPublished && x.Owner.Id == ownerId)),
                _ => new ProfileSummaryList(this.Where(x => x.IsPublished))
            };

            if (type.HasValue) output = output.FilterByType(type.Value);
            if (engines.HasValue) output = output.FilterByEngineCount(engines.Value);
            if (ownerId != null && ownerOnly) output = output.FilterByOwner(ownerId);
            if (searchTerms != null) output = output.FilterBySearch(searchTerms);

            return new ProfileSummaryList(output.OrderByDescending(x => x.LastUpdated));
        }

        public ProfileSummaryList(IEnumerable<ProfileSummary> profiles)
        {
            AddRange(profiles);
        }
    }

    public enum PublishedStatus
    {
        Published, Unpublished, PublishedOwner, UnpublishedOwner, All
    }
}
