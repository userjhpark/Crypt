create or replace PACKAGE PKG_UTILS AS
/******************************************************************************
   출처 : http://overoid.tistory.com/35
   NAME        : PKG_UTILS
   PURPOSE     : 시스템 공통으로 사용되는 함수, 프로시저 등을 제공하는 패키지 
   DESCRIPTION : UTILITY OPERATION PACKAGE
   REVISIONS   : 1.0
   Ver        Date        Author           Description
   ---------  ----------  ---------------  ------------------------------------
   1.0        2011-04-01  Jinook,lee       1. Created this package.
******************************************************************************/


    -------------------------------------------------------
    -- DESC : X, Y 값을 서로 BIT OR 연산한다.
    -- OR 연산은 두 값의 각 자릿수를 비교해, 둘 중 하나라도 1이 있다면 1을, 아니면 0을 계산한다
    -- x, y : 비트 연산할 숫자
    -- return   : bit or 연산한 결과값 
    --------------------------------------------------------
    FUNCTION BITOR(x NUMBER, y NUMBER)
    RETURN NUMBER DETERMINISTIC;

    -------------------------------------------------------
    -- DESC : X, Y 값을 서로 BIT XOR 연산한다.
    -- XOR 연산은 두 값의 각 자릿수를 비교해, 값이 같으면 0, 다르면 1을 계산한다
    -- x, y : 비트 연산할 숫자
    -- return   : BIT XOR 연산한 결과값 
    --------------------------------------------------------
    FUNCTION BITXOR(x NUMBER, y NUMBER)
    RETURN NUMBER DETERMINISTIC;

    -------------------------------------------------------
    -- DESC : X 값을 BIT NOT 연산한다.
    -- NOT 연산은 각 자릿수의 값을 반대로 바꾸는 연산이다.
    -- x : 비트 연산할 숫자
    -- return   : BIT NOT 연산한 결과값 
    --------------------------------------------------------
    FUNCTION BITNOT(x NUMBER)
    RETURN NUMBER DETERMINISTIC;

    -------------------------------------------------------
    -- DESC : 닷컴의 비트 컬럼값의 특정비트를 0으로 만드는 함수이다.
    -- 내부적으로 X & ^Y 로 연산한다.
    -- x, y : 비트 연산할 숫자
    -- return   : BIT 마스크 연산한 결과값 
    --------------------------------------------------------
    FUNCTION BIT_MASK(x NUMBER, y NUMBER)
    RETURN NUMBER DETERMINISTIC;

    -------------------------------------------------------
    -- DESC : 닷컴의 비트 컬럼값의 특정비트를 주어진 값으로 만드는 함수이다.
    -- x,y : 비트 연산할 숫자
    -- z : x값의 y자리수를 값을 설정할 값 (0 or 1)
    -- return   : BIT SET 연산한 결과값 
    --------------------------------------------------------
    FUNCTION BIT_SET(x NUMBER, y NUMBER, Z NUMBER)
    RETURN NUMBER DETERMINISTIC;

END PKG_UTILS;
/
create or replace PACKAGE BODY PKG_UTILS AS
/******************************************************************************
   NAME        : PKG_UTILS
   PURPOSE     : 시스템 공통으로 사용되는 함수, 프로시저 등을 제공하는 패키지 
   DESCRIPTION : UTILITY OPERATION PACKAGE
   REVISIONS   : 1.0
   Ver        Date        Author           Description
   ---------  ----------  ---------------  ------------------------------------
   1.0        2011-04-01  Jinook,lee       1. Created this package.
******************************************************************************/


    -------------------------------------------------------
    -- DESC : X, Y 값을 서로 BIT OR 연산한다.
    -- OR 연산은 두 값의 각 자릿수를 비교해, 둘 중 하나라도 1이 있다면 1을, 아니면 0을 계산한다
    -- x, y : 비트 연산할 숫자
    -- return   : bit or 연산한 결과값 
    --------------------------------------------------------
    FUNCTION BITOR(x NUMBER, y NUMBER)
    RETURN NUMBER DETERMINISTIC
    IS
    BEGIN
        RETURN x + y - BITAND(x, y);
    END;

    -------------------------------------------------------
    -- DESC : X, Y 값을 서로 BIT XOR 연산한다.
    -- XOR 연산은 두 값의 각 자릿수를 비교해, 값이 같으면 0, 다르면 1을 계산한다
    -- x, y : 비트 연산할 숫자
    -- return   : BIT XOR 연산한 결과값 
    --------------------------------------------------------
    FUNCTION BITXOR(x NUMBER, y NUMBER)
    RETURN NUMBER DETERMINISTIC
    IS
    BEGIN
        RETURN BITOR(x,y) - BITAND(x,y);
    END;

    -------------------------------------------------------
    -- DESC : X 값을 BIT NOT 연산한다.
    -- NOT 연산은 각 자릿수의 값을 반대로 바꾸는 연산이다.
    -- x : 비트 연산할 숫자
    -- return   : BIT NOT 연산한 결과값 
    --------------------------------------------------------
    FUNCTION BITNOT(x NUMBER)
    RETURN NUMBER DETERMINISTIC
    IS
    BEGIN
        RETURN (0 - x) - 1;
    END;

    -------------------------------------------------------
    -- DESC : 닷컴의 비트 컬럼값의 특정비트를 0으로 만드는 함수이다.
    -- 내부적으로 X & ^Y 로 연산한다.
    -- x, y : 비트 연산할 숫자
    -- return   : BIT 마스크 연산한 결과값 
    --------------------------------------------------------
    FUNCTION BIT_MASK(x NUMBER, y NUMBER)
    RETURN NUMBER DETERMINISTIC
    IS
    BEGIN
        RETURN BITAND(x, BITNOT(y));
    END;


    -------------------------------------------------------
    -- DESC : 닷컴의 비트 컬럼값의 특정비트를 주어진 값으로 만드는 함수이다.
    -- x,y : 비트 연산할 숫자
    -- z : x값의 y자리수를 값을 설정할 값 (0 or 1)
    -- return   : BIT SET 연산한 결과값 
    --------------------------------------------------------
    FUNCTION BIT_SET(x NUMBER, y NUMBER, z NUMBER)
    RETURN NUMBER DETERMINISTIC
    IS
    BEGIN
        IF z = 1 THEN
            RETURN BITOR(x, y);
        ELSIF z = 0 THEN
            RETURN BITAND(x, BITNOT(y));
        END IF;
    END;

END PKG_UTILS;
/