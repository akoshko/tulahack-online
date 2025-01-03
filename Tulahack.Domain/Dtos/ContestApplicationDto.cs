using System.ComponentModel.DataAnnotations;
using CommunityToolkit.Mvvm.ComponentModel;

// ReSharper disable once CheckNamespace
namespace Tulahack.Dtos;

[System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "13.18.2.0 (NJsonSchema v10.8.0.0 (Newtonsoft.Json v13.0.0.0))")]
public partial class ContestApplicationDto : ObservableValidator
{
    [ObservableProperty]
    private Guid _id;

    [ObservableProperty]
    [NotifyDataErrorInfo]
    [Required]
    [StringLength(64, MinimumLength = 1)]
    private string _participatorName;

    [ObservableProperty]
    [NotifyDataErrorInfo]
    [Required]
    [StringLength(64, MinimumLength = 1)]
    private string _participatorSurname;

    [ObservableProperty]
    [NotifyDataErrorInfo]
    [Required]
    [StringLength(64, MinimumLength = 1)]
    private string _participatorMiddleName;

    [ObservableProperty]
    [NotifyDataErrorInfo]
    [Required]
    [Range(14, 100)]
    private string _participatorAge;

    [ObservableProperty]
    [NotifyDataErrorInfo]
    [Required]
    private string _participatorTelegram;

    [ObservableProperty]
    [NotifyDataErrorInfo]
    [Required]
    [StringLength(64, MinimumLength = 1)]
    private string _participatorPhone;

    [ObservableProperty]
    [NotifyDataErrorInfo]
    [Required]
    private string _participatorAllocation;

    [ObservableProperty]
    [NotifyDataErrorInfo]
    [Required]
    private string _participatorCity;

    [ObservableProperty]
    [NotifyDataErrorInfo]
    [Required]
    private string _teamName;

    [ObservableProperty]
    private Guid? _existingTeamId;

    [ObservableProperty]
    [Required]
    [Range(2, 5)]
    private int _participatorsCount = 1;

    [ObservableProperty]
    [Required]
    private Guid _teamLeaderId;

    [ObservableProperty]
    [Required]
    private int _section;

    [ObservableProperty]
    [Required]
    private FormOfParticipationTypeDto _formOfParticipation;

    [ObservableProperty]
    [NotifyDataErrorInfo]
    [Required]
    private bool _isConsentChecked;

    [ObservableProperty]
    private bool _joinExistingTeam;

    [ObservableProperty]
    private string _aboutTeam;

    [ObservableProperty]
    private ApprovalStatusDto _approvalStatus;

    [ObservableProperty]
    private string _statusJustification;

    public void Validate() => ValidateAllProperties();
}