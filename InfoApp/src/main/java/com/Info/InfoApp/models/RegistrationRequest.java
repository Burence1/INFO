package com.Info.InfoApp.models;

import lombok.AllArgsConstructor;
import lombok.EqualsAndHashCode;
import lombok.Getter;
import lombok.ToString;

@Getter
@EqualsAndHashCode
@AllArgsConstructor
@ToString
public class RegistrationRequest {
    private final String userName;
    private final String email;
    private final String password;
}
