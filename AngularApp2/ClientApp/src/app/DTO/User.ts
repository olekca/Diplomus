import { Data } from "popper.js";

export interface UserDTO {
  UserId: number;
  UserName: string;
  Email: string;
  Password: string;
  Role: string;
  DayOfBirth: Data;
  UserImg: string;
}
