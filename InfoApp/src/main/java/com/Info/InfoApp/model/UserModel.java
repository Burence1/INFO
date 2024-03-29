package com.Info.InfoApp.model;

import lombok.AllArgsConstructor;
import lombok.Data;
import lombok.NoArgsConstructor;

@Data
@AllArgsConstructor
@NoArgsConstructor
public class UserModel {
    private String userName;
    private String firstName;
    private String lastName;
    private String email;
    private String password;
    private String matchingPassword;
}
