//using FYP_MS.Data;
//using FYP_MS.HelperClasses;
//using FYP_MS.Models;
//using FYP_MS.Services;
//using FYP_MS.Validations1;
//using FYP_MS.ViewModels.Base;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Windows.Input;

//namespace FYP_MS.ViewModels.Students;

//public sealed class AddStudentViewModel : ViewModelBase
//{
//    private readonly IStudentRepository _repo;
//    private readonly IDialogService _dialog;

//    public AddStudentViewModel(IStudentRepository repo, IDialogService dialog)
//    {
//        _repo = repo;
//        _dialog = dialog;

//        // populate gender list once
//        Genders = Lookup.getGenders();
//        SelectedGender = Genders.FirstOrDefault();

//        SaveCmd = new RelayCommand(_ => Save(), _ => CanSave());
//        CancelCmd = new RelayCommand(_ => _dialog.Close(this));
//    }

//    // ── Bindable properties ───────────────────────────────────────────────
//    private string _firstName = "";
//    public string FirstName { get => _firstName; set => Set(ref _firstName, value); }

//    private string _lastName = "";
//    public string LastName { get => _lastName; set => Set(ref _lastName, value); }

//    private string _contactNo = "";
//    public string ContactNo { get => _contactNo; set => Set(ref _contactNo, value); }

//    private string _regNo = "";
//    public string RegNo { get => _regNo; set => Set(ref _regNo, value); }

//    private string _email = "";
//    public string Email { get => _email; set => Set(ref _email, value); }

//    private DateTime _dob = DateTime.Today;
//    public DateTime Dob { get => _dob; set => Set(ref _dob, value); }

//    public IList<string> Genders { get; }
//    private string? _selectedGender;
//    public string? SelectedGender { get => _selectedGender; set => Set(ref _selectedGender, value); }

//    // ── Commands ─────────────────────────────────────────────────────────
//    public ICommand SaveCmd { get; }
//    public ICommand CancelCmd { get; }

//    // ── Helpers ──────────────────────────────────────────────────────────
//    bool CanSave() =>
//        ValidationsHelper.name(FirstName) &&
//        ValidationsHelper.name(LastName) &&
//        ValidationsHelper.contact(ContactNo) &&
//        ValidationsHelper.email(Email) &&
//        ValidationsHelper.age16plus(Dob);

//    async void Save()
//    {
//        try
//        {
//            var genderId = Lookup.getIndexFromValue(SelectedGender!);

//            await _repo.AddAsync(new Student
//            {
//                FirstName = FirstName,
//                LastName = LastName,
//                ContactNo = ContactNo,
//                Email = Email,
//                Dob = Dob,
//                GenderId = genderId
//            }, RegNo);

//            _dialog.Close(this);
//        }
//        catch (Exception ex)
//        {
//            _dialog.ShowError("Error saving record:\n\n" + ex.Message);
//        }
//    }
//}
