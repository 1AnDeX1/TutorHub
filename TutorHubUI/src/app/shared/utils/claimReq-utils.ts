export const claimReq ={
    teacherOnly: (c:any) => c.role == "Teacher",
    studentOnly: (c:any) => c.role == "Student",
    teacherAdminOnly: (c:any) => c.role == "Teacher" || c.role == "Admin"
}