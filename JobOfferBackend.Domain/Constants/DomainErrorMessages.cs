﻿
using System.Data;

namespace JobOfferBackend.Domain.Constants
{
    public static class DomainErrorMessages
    {
        public const string SKILL_REQUIRED = "SKILL_REQUIRED";
        public const string SKILL_NAME_REQUIRED = "SKILL_NAME_REQUIRED";
        public const string YEAR_REQUIRED = "YEAR_REQUIRED";
        public const string DATE_REQUIRED = "DATE_REQUIRED";
        public const string APPLICANT_REQUIRED = "APPLICANT_REQUIRED";
        public const string NAME_REQUIRED = "NAME_REQUIRED";
        public const string ACTIVITY_REQUIRED = "ACTIVITY_REQUIRED";
        public const string COMPANY_REQUIRED = "COMPANY_REQUIRED";
        public const string RECRUITER_REQUIRED = "RECRUITER_REQUIRED";
        public const string COMPANY_INVALID = "COMPANY_INVALID";
        public const string POSITION_REQUIRED = "POSITION_REQUIRED";
        public const string FROM_REQUIRED = "FROM_REQUIRED";
        public const string TO_REQUIRED_WHEN_ISNOT_CURRENT_JOB = "TO_REQUIRED_WHEN_ISNOT_CURRENT_JOB";
        public const string JOB_HISTORY_REPEATED = "JOB__HISTORY_REPEATED";        
        public const string JOB_DOES_NOT_EXISTS = "JOB_DOES_NOT_EXISTS";
        public const string STUDY_REPEATED = "STUDY_REPEATED";
        public const string ABILITY_REPEATED = "ABILITY_REPEATED";
        public const string IDENTITY_CARD_REQUIRED = "IDENTITY_CARD_REQUIRED";
        public const string FIRST_NAME_REQUIRED = "FIRST_NAME_REQUIRED";
        public const string LAST_NAME_REQUIRED = "LAST_NAME_REQUIRED";
        public const string COMPANY_REPEATED = "COMPANY_REPEATED";
        public const string INSTITUTION_REQUIRED = "INSTITUTION_REQUIRED";
        public const string TITLE_REQUIRED = "TITLE_REQUIRED";
        public const string APPLICANT_ALREADY_REQUESTED_JOB_OFFER = "APPLICANT_ALREADY_REQUESTED_JOB_OFFER";
        public const string APPLICANT_ALREADY_ACCEPTED = "APPLICANT_ALREADY_ACCEPTED";
        public const string APPLICANT_ALREADY_OFFERED = "APPLICANT_ALREADY_OFFERED";
        public const string APPLICANT_DOES_NOT_EXISTS = "APPLICANT_DOES_NOT_EXISTS";
        public const string SKILL_REQUIRED_ALREADY_EXISTS = "SKILL_REQUIRED_ALREADY_EXISTS";
        public const string PERSON_DOES_NOT_HAVE_ALL_MANDATORY_SKILLS = "PERSON_DOES_NOT_HAVE_ALL_MANDATORY_SKILLS";
        public const string PERSON_ALREADY_APLIED_TO_JOB_OFFER = "PERSON_ALREADY_APLIED_TO_JOB_OFFER";
        public const string INVALID_RECRUITER = "INVALID_RECRUITER";
        public const string RECRUITER_WHO_SAVE_THE_JOBOFFER_SHOULD_BE_THE_SAME_THAN_CREATED_IT = "RECRUITER_WHO_SAVE_THE_JOBOFFER_SHOULD_BE_THE_SAME_THAN_CREATED_IT";
        public const string JOBOFFER_WAS_MODIFIED_BEFORE_THIS_UPDATE = "JOBOFFER_WAS_MODIFIED_BEFORE_THIS_UPDATE";
        public const string ACCOUNT_DOES_NOT_EXISTS = "ACCOUNT_DOES_NOT_EXISTS";
        public const string CONTRACT_INFORMATION_EMPTY = "CONTRACT_INFORMATION_EMPTY";
        public const string SKILL_NAME_ALREADY_EXISTS = "SKILL_NAME_ALREADY_EXISTS";
        public const string ONLY_WORKINPROGRESS_JOBOFFERS_CAN_BE_PUBLISHED = "ONLY_WORKINPROGRESS_JOBOFFERS_CAN_BE_PUBLISHED";
    }
}
