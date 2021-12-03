## Mars.Rover
Mars.Rover .Net 5.x support !

## IoC
ASP.NET Core Dependency 

## Principles
SOLID <br/>
Domain Driven Design

## Persistance
EntityFramework Core<br/>
Dapper<br/>

## Object Mappers
AutoMapper

## Cache
In-Memory
Redis

## Object Validation
FluentValidation

## Log
Serilog support
Elasticsearch
Kibana

## Documentation
Swagger

## Benefits
 - Conventional Dependency Registering
 - Async await first 
 - Multi Tenancy
 - GlobalQuery Filtering
 - Domain Driven Design Concepts
 - Repository and UnitofWork pattern implementations
 - Object to object mapping with abstraction
 - Net Core 3.x support
 - Auto object validation support
 - Aspect Oriented Programming
 - Simple Usage
 - Modularity
 
 ***Run Migrations***

     Replace Id and Password fields with your information in MarsRoverContext.cs
            if (!optionsBuilder.IsConfigured)
                optionsBuilder.UseSqlServer(
                    "Data Source=DESKTOP-9OSHF57;Database=Mars.Rover;User Id=*****;Password=*****");

     Run in terminal 
	 --   update-database	

*** Using Swagger***
https://localhost:44364/swagger/index.html

**** Basic Request****
 Select /api/Navigate/NavigateMarsRover as POST Method
Example Request;
{
  "parameters": [
     "5 5", "1 2 N", "LMLMLMLMM", "3 3 E", "MMRMMRMRRM"
  ]
}
Example Response;
{
  "data": [
    {
      "responseString": "1 3 N"
    },
    {
      "responseString": "5 1 E"
    }
  ],
  "message": "NavigateMarsRover Operation is succeeded.",
  "information": {
    "trackId": "fa6c4e39-bdf6-4394-bc5f-dd6ed1bbf052"
  },
  "rc": "RC0000"
} 

***Basic Usage***

     WebHost.CreateDefaultBuilder(args)
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseStartup<Startup>();
                         
***MultiTenancy Activation***

    var connectionString = config["mysqlconnection:connectionString"];
            services.AddDbContext<Context>(o => o.UseSqlServer(connectionString));
***Conventional Registration***	 	

      services.AddScoped<IUserStoreService, UserStoreService>();
                             ...
                         })

***FluentValidators Activation***

     services.ConfigureFluentValidation();

     public static void ConfigureFluentValidation(this IServiceCollection services)
        {
            //services.AddTransient<IValidator<IValidator<Domain.Context.Entities.Galley>>, GalleyValidatorValidator>();
            services.AddTransient<IValidator<RefType>, RefTypeValidator>();
            services.AddTransient<IValidator<RefType>, RefTypeValidator>();
                RuleFor(t => t.Name).NotEmpty().MinimumLength(3);
                ...
            }
        }

  
***AutoMapper Activation***

	 services.AddAutoMapper();
	 
***Swagger Activation***

	 services.ConfigureSwagger();


***Serilog Activation***
 

        services.ConfigureLogger(Configuration);
		
		 Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .Enrich.WithProperty("Application", "Mars.Rover.Presentation.API")
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .MinimumLevel.Override("System", LogEventLevel.Warning)
                //.WriteTo.File(new JsonFormatter(), "log.json")
                //.ReadFrom.Configuration(configuration)
                .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri("http://localhost:9200"))
                {
                    AutoRegisterTemplate = true,
                    AutoRegisterTemplateVersion = AutoRegisterTemplateVersion.ESv6,
                    FailureCallback = e => Console.WriteLine("Unable to submit event " + e.MessageTemplate),
                    EmitEventFailure = EmitEventFailureHandling.WriteToSelfLog |
                                       EmitEventFailureHandling.WriteToFailureSink |
                                       EmitEventFailureHandling.RaiseCallback,
                    FailureSink = new FileSink("log.json", new JsonFormatter(), null)
                })
                .MinimumLevel.Verbose()
                .CreateLogger();
            Log.Information("WebApi is Starting...");
		
		

***Interceptors Activation***

     public ErrorHandlingMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context /* other dependencies */)
        {
            try
            {
                Log.Write(LogEventLevel.Information, "Service path is:" + context.Request.Path.Value,
                    context.Request.Body);
                await next(context);
            }
            catch (HttpRequestException ex)
            {
                Log.Write(LogEventLevel.Error, ex.Message, "Service path is:" + context.Request.Path.Value, ex);
                await HandleExceptionAsync(context, ex);
            }
            catch (AuthenticationException ex)
            {
                Log.Write(LogEventLevel.Error, ex.Message, "Service path is:" + context.Request.Path.Value, ex);
                await HandleExceptionAsync(context, ex);
            }
            catch (BusinessException ex)
            {
                Log.Write(LogEventLevel.Error, ex.Message, "Service path is:" + context.Request.Path.Value, ex);
                await HandleExceptionAsync(context, ex);
            }
            catch (Exception ex)
            {
                Log.Write(LogEventLevel.Error, ex.Message, ex.Source, ex.TargetSite, ex);
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, object exception)
        {
            var code = HttpStatusCode.InternalServerError; // 500 if unexpected
            var message = string.Empty;
            var RC = string.Empty;

            if (exception.GetType() == typeof(HttpRequestException))
            {
                code = HttpStatusCode.NotFound;
                RC = ResponseMessage.NotFound;
                message = BusinessException.GetDescription(RC);
            }
            else if (exception.GetType() == typeof(AuthenticationException))
            {
                code = HttpStatusCode.Unauthorized;
                RC = ResponseMessage.Unauthorized;
                message = BusinessException.GetDescription(RC);
            }
            else if (exception.GetType() == typeof(BusinessException))
            {
                var businesException = (BusinessException) exception;
                message = BusinessException.GetDescription(businesException.RC, businesException.param1);
                code = HttpStatusCode.InternalServerError;
                RC = businesException.RC;
            }
            else if (exception.GetType() == typeof(Exception))
            {
                code = HttpStatusCode.BadRequest;
                RC = ResponseMessage.BadRequest;
                message = BusinessException.GetDescription(RC);
            }

            var response = new Error
            {
                Message = message,
                RC = RC
            };
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int) code;
            return context.Response.WriteAsync(JsonConvert.SerializeObject(response));
        }

***AggregateRoot definitions***

   public interface IRepository<T> where T : class
    {
        /// <summary>
        ///     UserManager
        /// </summary>
        UserManager<IdentityUser<string>> UserManager { get; set; }

        /// <summary>
        ///     RoleManager
        /// </summary>
        RoleManager<IdentityUserRole<string>> RoleManager { get; set; }

        IEnumerable<T> FindAll();
        IEnumerable<T> FindByCondition(Expression<Func<T, bool>> expression);
        void Create(T entity);
        void Update(T entity);
        void Delete(T entity);
        void DeleteBulk(IEnumerable<T> entity);
        void InsertBulk(IEnumerable<T> entity);
        void UpdateBulk(IEnumerable<T> entity);
        void Save();
        T GetByKey(int key);
        T GetByKey(string key);
        T GetByKey(object key);
        /// <summary>
        /// Query Method
        /// </summary>
        /// <returns>RepositoryQueryHelper (Sorgu Yardımcı Sınıfı)</returns>
        IRepositoryQueryHelper<T> Query();

        /// <summary>
        /// To Set Data to Table, Definition Some Helper Parameters
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="orderBy"></param>
        /// <param name="includeProperties"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        IQueryable<T> Get(
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            List<Expression<Func<T, object>>> includeProperties = null,
            int? page = null,
            int? pageSize = null);
    }
	
	
        
***Application Service definitions***

    public class ApplicationService : IApplicationService
    {
        private INavigateService navigateService { get; set; }

        public ApplicationService(INavigateService navigateService)
        {
            this.navigateService = navigateService;
        }

        /// <summary>
        /// Navigates the mars rover.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public async Task<ResponseListDTO<NavigateMarsRoverResponseDTO>> NavigateMarsRover(NavigateMarsRoverRequestDTO request)
        {
            List<NavigateMarsRoverResponseDTO> responseList = new List<NavigateMarsRoverResponseDTO>();

            responseList = await this.navigateService.NavigateMarsRover(request);

            return await CreateAsyncResponse<NavigateMarsRoverResponseDTO>.Return(responseList, "NavigateMarsRover");
        }
    }
}
	
	
***Domain Service definitions***

    public interface INavigateService
    {
        /// <summary>
        /// Navigates the mars rover.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        Task<List<NavigateMarsRoverResponseDTO>> NavigateMarsRover(NavigateMarsRoverRequestDTO request);
    }
		
***IRepository Interface***		
    public interface IRepository<T> where T : class
    {
        /// <summary>
        ///     UserManager
        /// </summary>
        UserManager<IdentityUser<string>> UserManager { get; set; }

        /// <summary>
        ///     RoleManager
        /// </summary>
        RoleManager<IdentityUserRole<string>> RoleManager { get; set; }

        IEnumerable<T> FindAll();
        IEnumerable<T> FindByCondition(Expression<Func<T, bool>> expression);
        void Create(T entity);
        void Update(T entity);
        void Delete(T entity);
        void DeleteBulk(IEnumerable<T> entity);
        void InsertBulk(IEnumerable<T> entity);
        void UpdateBulk(IEnumerable<T> entity);
        void Save();
        T GetByKey(int key);
        T GetByKey(string key);
        T GetByKey(object key);
        /// <summary>
        /// Query Method
        /// </summary>
        /// <returns>RepositoryQueryHelper (Sorgu Yardımcı Sınıfı)</returns>
        IRepositoryQueryHelper<T> Query();

        /// <summary>
        /// To Set Data to Table, Definition Some Helper Parameters
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="orderBy"></param>
        /// <param name="includeProperties"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        IQueryable<T> Get(
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            List<Expression<Func<T, object>>> includeProperties = null,
            int? page = null,
            int? pageSize = null);
    }

***Dapper Repository definitions***
 

  public class RefTypeDapperRepository : IDeliveryPlanDetailDapperRepository
    {
        private static string cnString =
            "Data Source = 10.22.0.201; Initial Catalog = test; Persist Security Info=True;User ID = sa; Password=User123!!;";
			...
			
			 public IEnumerable<RefType> GetRefType(RefTypeRequestDTO request)
        {
            IEnumerable<RefType> entities = new List<RefType>();
            using (IDbConnection con = new SqlConnection(cnString))
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();

                DynamicParameters parameters=new DynamicParameters();
                parameters.Add("@id", request.Id);
                parameters.Add("@insertDate",request.InsertDate);
               

                StringBuilder query= new StringBuilder();
                query.Append("select * from RefType rt");
                query.Append(" where");
                query.Append(" rt.Id == @id)");
                query.Append(" rt.InsertDate >= @insertDate ");

                entities = con.Query<RefTYpe>(query.ToString(),parameters).ToList();
            }

            return entities;
        }
			}


***EntityFrameworkCore definitions***
   

    public class MarsRoverContext : IdentityDbContext<IdentityUser>
    {
        public MarsRoverContext()
        {
        }

        public MarsRoverContext(DbContextOptions<MarsRoverContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Plataeu> Plataeus { get; set; }
        public virtual DbSet<Surface> Surfaces { get; set; }
        public virtual DbSet<MarsRover> MarsRovers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
                optionsBuilder.UseSqlServer(
                    "Data Source=localhost;Initial Catalog=Mars.Rover; Persist Security Info=True;User ID=sa;Password=User123**;MultipleActiveResultSets=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
     

***Log Service definitions***

              public static ResponseBaseDTO Make(T entity, string methodName)
        {
           
            string message = string.Empty;
            if (entity!=null)
                message = ResponseMessage.GetDescription(ResponseMessage.Success, methodName);
            else
                message = ResponseMessage.GetDescription(ResponseMessage.NotFound, methodName);
            ResponseBaseDTO response= new ResponseBaseDTO
            {
                Data = entity,
                Message = message,
                Information = new Information
                {
                    TrackId = Guid.NewGuid().ToString()
                },
                RC = ResponseMessage.Success
            };
           Log.Write(LogEventLevel.Information, message,response);
           return response;
        }

        public static ResponseBaseDTO Make(IEnumerable<T> entity, string methodName)
        {
            string message = string.Empty;
            if (entity.Count() > 0)
                message = ResponseMessage.GetDescription(ResponseMessage.Success, methodName);
            else
                message = ResponseMessage.GetDescription(ResponseMessage.NotFound, methodName);

            ResponseBaseDTO response = new ResponseBaseDTO
            {
                Data = entity,
                Message = message,
                Information = new Information
                {
                    TrackId = Guid.NewGuid().ToString()
                },
                RC = ResponseMessage.Success
            };
            Log.Write(LogEventLevel.Information, message, response);
            return response;
        }
