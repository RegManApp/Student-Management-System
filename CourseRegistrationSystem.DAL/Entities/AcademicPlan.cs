
public class AcademicPlan
{
    private string PlanID ;
    private string majorname ;
    private int Creds ;

    private List<Course> courses;

    public void SetPlanID(string x)
     {
        PlanID = x;
     }

    public void Setmajorname(string x)
     {
        majorname = x;
     }

    public void SetCreds(int x)
     {
        Creds = x;
     }

    public PlanID GetPlan()
     {
      return PlanID;
     }

    public majorname GetMajorName()
     {
      return majorname;
     }

    public Creds GetCreds()
     {
      return Creds;
     }

    public AcademicPlan(string x, string y, int z)
    {
        PlanID = x;
        majorname = y;
        Creds = z;
        courses = new List<Course>();
    }

    public void AddCourse(Course x)
    {
        if (x != null)
            courses.Add(x);
    }

    public int GetTotalCredits()
    {
        int total = 0;

        foreach (var c in courses)
        {
            total += c.Creds;
        }

        return total;
    }
}
