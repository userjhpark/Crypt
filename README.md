# Crypt
[PHP]"ADOdb crypt.inc.php" MD5Crypt Class의 암호화 및 복호화가 가능한 라이블러리를 C#과 Oracle(PL/SQL)용도로 제작한 라이블러리

C# 

PHP Source : ADOdb crypt.inc.php ( http://adodb.org/ )
Oracle : 비트연산용 PKG_UTILS( http://overoid.tistory.com/35 ) 참조
C# : 최종 버전은 HxCore.HxCrypt(https://github.com/userjhpark/HxCore) 참고

- 암호화 : HxCrypt.Encrypt(문자열, 키값) //호출 할 때마다 다른 값이 리턴됨
- 복호화 : HxCrypt.Decrypt(암호 문자열, 키값)

암호화 문자열과 키값은 반드시 키보드에 존재하는 영문+숫자+특수키 값만을 정상적으로 사용 가능하며, 기타 다른 문자(특수문자, 한글, …)들은 문자 인코딩 타입과 플랫폼에 따라 상이한 결과가 나오므로 주의가 필요함.
