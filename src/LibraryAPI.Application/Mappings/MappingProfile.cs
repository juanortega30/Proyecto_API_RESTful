using AutoMapper;
using LibraryAPI.Application.DTOs.Books;
using LibraryAPI.Application.DTOs.Loans;
using LibraryAPI.Application.DTOs.Reservations;
using LibraryAPI.Application.DTOs.Users;
using LibraryAPI.Domain.Entities;

namespace LibraryAPI.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {

        CreateMap<Book, BookDto>()
            .ForMember(d => d.GeneroName, o => o.MapFrom(s => s.Genero.Name))
            .ForMember(d => d.Authors, o => o.MapFrom(s =>
                s.BookAuthors.Select(ba => ba.Author.FirstName + " " + ba.Author.LastName).ToList()))
            .ForMember(d => d.AvailableCopies, o => o.MapFrom(s =>
                s.Copies.Count(c => c.IsAvailable)));

        CreateMap<CreateBookDto, Book>();

        CreateMap<User, UserDto>()
            .ForMember(d => d.Role, o => o.MapFrom(s => s.Role.ToString()));
        CreateMap<CreateUserDto, User>();

        CreateMap<Loan, LoanDto>()
            .ForMember(d => d.UserFullName, o => o.MapFrom(s =>
                s.User.FirstName + " " + s.User.LastName))
            .ForMember(d => d.BookTitle, o => o.MapFrom(s => s.Copy.Book.Title))
            .ForMember(d => d.Status, o => o.MapFrom(s => s.Status.ToString()));


        CreateMap<Reservation, ReservationDto>()
            .ForMember(d => d.UserFullName, o => o.MapFrom(s =>
                s.User.FirstName + " " + s.User.LastName))
            .ForMember(d => d.BookTitle, o => o.MapFrom(s => s.Book.Title))
            .ForMember(d => d.Status, o => o.MapFrom(s => s.Status.ToString()));
    }
}