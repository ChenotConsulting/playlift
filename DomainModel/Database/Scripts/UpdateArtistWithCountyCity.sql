USE [TRM_Test]
GO

UPDATE [dbo].[Artist]
   SET [ArtistName] = <ArtistName, nvarchar(250),>
      ,[UserId] = <UserId, int,>
      ,[Email] = <Email, nvarchar(250),>
      ,[Website] = <Website, nvarchar(250),>
      ,[SoundCloud] = <SoundCloud, nvarchar(250),>
      ,[MySpace] = <MySpace, nvarchar(250),>
      ,[Facebook] = <Facebook, nvarchar(250),>
      ,[Twitter] = <Twitter, nvarchar(250),>
      ,[TermsAndConditionsAccepted] = <TermsAndConditionsAccepted, bit,>
      ,[ProfileImage] = <ProfileImage, nvarchar(260),>
      ,[PRS] = <PRS, bit,>
      ,[CreativeCommonsLicence] = <CreativeCommonsLicence, bit,>
      ,[Active] = <Active, bit,>
      ,[Bio] = <Bio, nvarchar(500),>
      ,[CountyCityId] = <CountyCityId, int,>
 WHERE <Search Conditions,,>
GO

