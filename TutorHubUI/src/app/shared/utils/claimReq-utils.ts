export const claimReq ={
    teacherOnly: (c:any) => c.role == "Teacher",
    studentOnly: (c:any) => c.role == "Student",
    adminOnly: (c:any) => c.role == "Admin",
    teacherAdminOnly: (c:any) => c.role == "Teacher" || c.role == "Admin"
}