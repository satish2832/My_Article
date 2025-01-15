using AppRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TechPortalWeb.Models;

namespace TechPortalWeb.Helpers
{
    public static class MapperHelper
    {
        public static Destination Map<Source, Destination>(Source source, Destination destination = default(Destination))
            where Source : class
            where Destination : class, new()
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (destination == null) destination = new Destination();

            if (source is EnquiryFormModel && destination is CandidateEnquiry)
            {
                return ConvertToCandidateEnquiry(source as EnquiryFormModel, destination as CandidateEnquiry) as Destination;
            }
            else if (source is CandidateEnquiry && destination is EnquiryFormModel)
            {
                return ConvertToEnquiryFormModel(source as CandidateEnquiry, destination as EnquiryFormModel) as Destination;
            }
            else if (source is FollowUpModel && destination is CandidateEnquiryFollowup)
            {
                return ConvertToCandidateEnquiryFollowup(source as FollowUpModel, destination as CandidateEnquiryFollowup) as Destination;
            }
            else if (source is CandidateEnquiryFollowup && destination is FollowUpModel)
            {
                return ConvertToFollowUpModel(source as CandidateEnquiryFollowup, destination as FollowUpModel) as Destination;
            }
            else if (source is Skillset && destination is SkillsetModel)
            {
                return ConvertToSkillsetModel(source as Skillset, destination as SkillsetModel) as Destination;
            }
            else if (source is ArticleModel && destination is Article)
            {
                return ConvertToArticle(source as ArticleModel, destination as Article) as Destination;
            }
            else if (source is Article && destination is ArticleModel)
            {
                return ConvertToArticleModel(source as Article, destination as ArticleModel) as Destination;
            }
            throw new InvalidOperationException($"Mapping from {typeof(Source)} to {typeof(Destination)} is not supported.");
        }

        private static Article ConvertToArticle(ArticleModel articleCreateModel, Article article)
        {
            if (articleCreateModel == null) throw new ArgumentNullException(nameof(articleCreateModel));
            if (article == null) article = new Article();

            article.Id = articleCreateModel.Id == Guid.Empty ? Guid.NewGuid() : articleCreateModel.Id;
            article.Title = articleCreateModel.Title;
            article.TitleURL = articleCreateModel.TitleURL;
            article.ContentFile = articleCreateModel.ContentFile;
            article.ContentFileURL = articleCreateModel.ContentFileURL;
            article.ArticleTypeId = articleCreateModel.ArticleTypeId;
            article.Tags = articleCreateModel.Tags;
            article.CreatedBy = Guid.Parse(Constants.GlobalUserId);
            article.CreateDT = DateTime.Now;
            article.UpdatedBy = Guid.Parse(Constants.GlobalUserId);
            article.UpdateDT = DateTime.Now;

            return article;
        }

        private static ArticleModel ConvertToArticleModel(Article article, ArticleModel articleCreateModel)
        {
            if (article == null) throw new ArgumentNullException(nameof(article));
            if (articleCreateModel == null) articleCreateModel = new ArticleModel();

            articleCreateModel.Id = article.Id;
            articleCreateModel.Title = article.Title;
            articleCreateModel.TitleURL = article.TitleURL;
            articleCreateModel.ContentFile = article.ContentFile;
            articleCreateModel.ContentFileURL = article.ContentFileURL;
            articleCreateModel.ArticleTypeId = article.ArticleTypeId;
            articleCreateModel.Tags = article.Tags;
            articleCreateModel.CreatedBy = Guid.Parse(Constants.GlobalUserId).ToString();
            articleCreateModel.CreatedDate = article.CreateDT;
            articleCreateModel.UpdatedBy = Guid.Parse(Constants.GlobalUserId).ToString();
            articleCreateModel.UpdatedDate = article.UpdateDT;

            return articleCreateModel;
        }

        private static CandidateEnquiry ConvertToCandidateEnquiry(EnquiryFormModel enquiryFormModel, CandidateEnquiry candidateEnquiry)
        {
            if (enquiryFormModel == null) throw new ArgumentNullException(nameof(enquiryFormModel));
            if (candidateEnquiry == null) candidateEnquiry = new CandidateEnquiry();

            candidateEnquiry.Id = Guid.NewGuid();
            candidateEnquiry.Name = enquiryFormModel.Name;
            candidateEnquiry.PhoneNumber = enquiryFormModel.PhoneNumber;
            candidateEnquiry.Email = enquiryFormModel.Email;
            candidateEnquiry.SkillsetId = Guid.Parse(enquiryFormModel.Skillset);
            candidateEnquiry.LocationId = Guid.Parse(Constants.GlobalLocationId);
            candidateEnquiry.Comments = enquiryFormModel.Comments;
            candidateEnquiry.CreatedBy = Guid.Parse(Constants.GlobalUserId);
            candidateEnquiry.CreateDT = DateTime.Now;
            candidateEnquiry.UpdatedBy = Guid.Parse(Constants.GlobalUserId);
            candidateEnquiry.UpdateDT = DateTime.Now;
            return candidateEnquiry;
        }

        private static EnquiryFormModel ConvertToEnquiryFormModel(CandidateEnquiry candidateEnquiry, EnquiryFormModel enquiryFormModel)
        {
            if (candidateEnquiry == null) throw new ArgumentNullException(nameof(candidateEnquiry));

            enquiryFormModel = new EnquiryFormModel
            {
                Id = candidateEnquiry.Id,
                Name = candidateEnquiry.Name,
                PhoneNumber = candidateEnquiry.PhoneNumber,
                Email = candidateEnquiry.Email,
                Skillset = candidateEnquiry.Skillset.Name,
                Comments = candidateEnquiry.Comments,
                followUps = candidateEnquiry.CandidateEnquiryFollowups.Select(x => new FollowUpModel()
                {
                    Id = x.Id,
                    CandidateEnquiryId = x.CandidateEnquiryId,
                    Content = x.Text,
                    LastUpdatedBy = x.UpdatedBy.ToString(),
                    LastUpdatedOn = x.UpdateDT ?? DateTime.Now,
                }).ToList(),
            };
            return enquiryFormModel;
        }

        private static SkillsetModel ConvertToSkillsetModel(Skillset skillset, SkillsetModel skillsetModel)
        {
            if (skillset == null) throw new ArgumentNullException(nameof(Skillset));
            if (skillsetModel == null) skillsetModel = new SkillsetModel();

            skillsetModel.Id = skillset.Id;
            skillsetModel.Name = skillset.Name;

            return skillsetModel;
        }

        private static CandidateEnquiryFollowup ConvertToCandidateEnquiryFollowup(FollowUpModel followUpModel, CandidateEnquiryFollowup candidateEnquiryFollowup)
        {
            if (followUpModel == null) throw new ArgumentNullException(nameof(FollowUpModel));
            if (candidateEnquiryFollowup == null) candidateEnquiryFollowup = new CandidateEnquiryFollowup();

            candidateEnquiryFollowup.Id = Guid.NewGuid();
            candidateEnquiryFollowup.Text = followUpModel.Content;
            candidateEnquiryFollowup.CandidateEnquiryId = followUpModel.CandidateEnquiryId;
            candidateEnquiryFollowup.CreatedBy = Guid.Parse(Constants.GlobalUserId);
            candidateEnquiryFollowup.CreateDT = DateTime.Now;
            candidateEnquiryFollowup.UpdatedBy = Guid.Parse(Constants.GlobalUserId);
            candidateEnquiryFollowup.UpdateDT = DateTime.Now;

            return candidateEnquiryFollowup;
        }

        private static FollowUpModel ConvertToFollowUpModel(CandidateEnquiryFollowup candidateEnquiryFollowup, FollowUpModel followUpModel)
        {
            if (candidateEnquiryFollowup == null) throw new ArgumentNullException(nameof(candidateEnquiryFollowup));

            followUpModel = new FollowUpModel
            {
                Content = candidateEnquiryFollowup.Text,
                CandidateEnquiryId = candidateEnquiryFollowup.CandidateEnquiryId,
                LastUpdatedBy = candidateEnquiryFollowup.UpdatedBy.ToString(),
                LastUpdatedOn = candidateEnquiryFollowup.UpdateDT ?? DateTime.Now
            };

            return followUpModel;
        }
    }
}