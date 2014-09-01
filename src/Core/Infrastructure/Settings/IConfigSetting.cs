namespace Lacjam.Core.Infrastructure.Settings
{
    public interface IConfigSetting
    {
        string NetTcpWcfBackEndAddress { get; set; }
        int NetTcpWcfBackEndPort { get; set; }
        string HttpWcfBackEndAddress { get; set; }
        string HttpWcfFrontEndAddress { get; set; }
        string ClientServicesEmail { get; set; }
        string DoNotReplyEmailAddress { get; set; }
        string Host { get; set; }
        string CmsMenuFolderPath { get; set; }
        string CmsMenuFileName { get; set; }
        string CertificateFriendlyName { get; set; }
        string ServiceIdentityDns { get; set; }
        string ExcludeInvestmentOptionIds { get; set; }
        bool ValidateXml { get; set; }
        bool LogQuoteCalculationMessageXmlInputs { get; set; }
        string EmailSubject { get; set; }
        string EQuoteWebServiceChallengerIdAccountsDissallowed { get; set; }
        bool DataExtractWebServiceEnabled { get; set; }
    }
}
