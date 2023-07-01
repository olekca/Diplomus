export interface ImgAccountDTO {
  UserId: number;
  UserImg: string;
}

export interface NameAccountDTO {
  UserId: number;
  UserName: string;
}

export interface EmailAccountDTO {
  UserId: number;
  Email: string;
}

export interface PasswordAccountDTO {
  UserId: number;
  PrevPassword: string;
  NewPassword: string;
}

export interface RoleAccountDTO {
  UserId: number;
  Secret: string;
}
